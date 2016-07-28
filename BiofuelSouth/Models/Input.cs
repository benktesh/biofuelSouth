using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BiofuelSouth.Enum;
using BiofuelSouth.Services;
using log4net;

namespace BiofuelSouth.Models
{
    public class Input : ICloneable
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int Id { get; set; }

        public Input()
        {
            General = new General();
            Storage = new Storage();
            Financial = new Financial();
            ProductionCost = new ProductionCostViewModel();
            CurrentStep = WizardStep.None;
        }

		
		public WizardStep CurrentStep { get; set; }

        public General General { get; set; }

        public Storage Storage { get; set; }

        public Financial Financial { get; set; }

        public ProductionCostViewModel ProductionCost { get; set; }

        public double[] GetCashFlow()
        {
            var duration = General.ProjectLife;
            double[] cashFlow = new double[duration.GetValueOrDefault()];

            //for each year
            //estimate expesese
            var expenses = GetExpenditures();
            var revenues = GetRevenues();

            //estimate reveneue
            //get net and insert into cashflow
            for (var i = 0; i < duration; i++)
            {
                cashFlow[i] = (double) (-expenses[i].TotalExpenses + revenues[i].TotalRevenue);
            }

            return cashFlow;
        }

        public Double GetNpv()
        {
            var cashFlow = GetCashFlow();
            var npv = Microsoft.VisualBasic.Financial.NPV(Financial.InterestRate/100, ref cashFlow);
            return npv;
        }

        public List<Expenditure> GetExpenditures()
        {
            var expenses = new List<Expenditure>();
            var storageCost = Constants.GetStorageCost(this);

            try
            { 
                var duration = General.ProjectLife ?? 10;
                var annualProductionCosts = GetAnnualProductionCosts();
                for (var i = 0; i < duration; i++)
                {
                    var expenditure = new Expenditure
                    {
                        Year = i,
                        AdministrativeCost = Financial.AdministrativeCost,
                        LandCost = General.LandCost.GetValueOrDefault(),
                        ProductionCost = (decimal) annualProductionCosts[i]
                    };

                    expenditure.StorageCost = storageCost != null ? storageCost[i] : 0;

                    expenditure.TotalExpenses = expenditure.AdministrativeCost + expenditure.LandCost +
                                                expenditure.ProductionCost;
                    expenditure.TotalExpenses = expenditure.TotalExpenses*(decimal) General.ProjectSize.GetValueOrDefault() +
                                                expenditure.StorageCost
                                                + Financial.LoanAmount*(decimal) (Financial.EquityLoanInterestRate/100);
                    expenses.Add(expenditure);

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }
            return expenses;
        }




        public IList<Revenue> GetRevenues()
        {
            var revenues = new List<Revenue>();
            try
            {
                var production = GetAnnualProductionList();
                var duration = General.ProjectLife;
                for (var i = 0; i < duration; i++)
                {
                    var revenue = new Revenue();
                    revenue.Year = i;
                    revenue.IncentivePayments = 0;
                    //if the current year is less than the years of incenptive payments, then there is a incentivepayment revenue;
                    if (i < Financial.YearsOfIncentivePayment)
                    {
                        revenue.IncentivePayments = Financial.IncentivePayment * (decimal) General.ProjectSize.GetValueOrDefault();
                    }
                    revenue.BiomassPrice = (decimal) (production[i] * (double) GetBiomassPrice());
                    revenue.TotalRevenue = (revenue.IncentivePayments + revenue.BiomassPrice);

                    revenues.Add(revenue);
                }
            }
            catch (Exception)
            {
                Log.Error("Revenues cannot be calcualted");
            }
            return revenues;
        }

        /// <summary>
        /// Returns total annual production for the project si;
        /// </summary>
        /// <returns></returns>
        public decimal GetAnnualProductivity()
        {
            return (decimal) (DataService.GetProductivityPerAcreForCropByGeoId(General.Category, General.County) * General.ProjectSize.GetValueOrDefault());
        }

        public double GetCostPerAcre()
        {
            if (ProductionCost.TotalProductionCost > 0)
                return (double)ProductionCost.TotalProductionCost;
            return (DataService.GetCostPerAcreForCropByGeoId(General.Category, General.County));
        }

        /// <summary>
        /// Provides Total Annual Cost i.e.[ per acre cost of production + land cost ] * project size
        /// </summary>
        /// <returns></returns>
        public double GetAnnualCost()
        {
            if (ProductionCost.TotalProductionCost > 0)
                return ((double)(ProductionCost.TotalProductionCost) + (double) General.LandCost.GetValueOrDefault()) *
                    General.ProjectSize.GetValueOrDefault();
            return (DataService.GetCostPerAcreForCropByGeoId(General.Category, General.County) + (double) General.LandCost.GetValueOrDefault()) * General.ProjectSize.GetValueOrDefault();
        }

        public decimal GetBiomassPrice()
        {
            if (Convert.ToInt32(General.BiomassPriceAtFarmGate) == 0)
            {
                General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(General.Category);
            }
            return General.BiomassPriceAtFarmGate.GetValueOrDefault();
        }

        public double GetAnnualRevenue()
        {
            if (Convert.ToInt32(General.BiomassPriceAtFarmGate) == 0)
            {
                General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(General.Category);
            }
            return (double) (GetAnnualProductivity() * General.BiomassPriceAtFarmGate.GetValueOrDefault());
        }


        public IList<double> GetAnnualProductionCosts()
        {
            
            var rotation = CropAttribute.GetRoationYears(General.Category);


            var annualProductionCost = new List<double>();

            var standardAnnualCost = GetCostPerAcre();

            if (rotation == 1)
            {
                // ReSharper disable once RedundantAssignment
                annualProductionCost.Select(delegate(double c)
                {
                    c = standardAnnualCost;
                    return c;
                    // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                }).ToList();
            }

            decimal sitePlantationFactor = 0.38M;
            decimal thinningFactor = 0.10M;
            decimal harvestFactor = 0.50M;
            decimal custodialFactor = 0.02M;

            if (ProductionCost.CanCustomize && ProductionCost.ProductionCosts.Any())
            {
                var total = ProductionCost.TotalProductionCost;
                var o3 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.Planting ||
                        m.ProductionCostType == ProductionCostType.SitePreparation).Sum(m => m.Amount)/total;
                if (o3 != null)
                    sitePlantationFactor =
                        (decimal) o3;

                var o2 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.CustodialManagement).Sum(m => m.Amount)/total;
                if (o2 !=
                    null)
                    thinningFactor =
                        (decimal) o2;

                var o = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.Harvesting).Sum(m => m.Amount)/total;
                if (o != null)
                    harvestFactor =
                        (decimal) o;

                var o1 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.CustodialManagement).Sum(m => m.Amount)/total;
                if (o1 !=
                    null)
                    custodialFactor =
                        (decimal) o1;
                Log.Info("Custom cost included");
            }
            else
            {
                sitePlantationFactor = 0.0M;
                thinningFactor = 0.0M;
                harvestFactor = 0.0M;
                custodialFactor = 1.0M;
            }

            //user factor
            //.38 for site preparatio and planting
            //.10 for thinnin
            //.50 for harvesting
            //.02 custodial management
            //var thinningYear = (int)Math.Ceiling(rotation / 2.0);

            var thinningYear = CropAttribute.GetThinningYear(General.Category);

            for (var i = 0; i < General.ProjectLife; i++)
            {

                if (i % rotation == 0) //plantation
                {
                    if (CropAttribute.CanCoppice(General.Category) && i > 0)
                    {
                        sitePlantationFactor = 0; 
                    }
                    annualProductionCost.Add(standardAnnualCost * (double)(sitePlantationFactor + custodialFactor));
                   
                   
                }
                else if (i > 0 && (i + 1) % rotation == 0) //harvest
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(harvestFactor + custodialFactor));
                   
                }
               
                else if(thinningYear !=null && (i > 0 && ((i + 1)%thinningYear == 0) || ((i % rotation)+1)%thinningYear ==0)) //thinning
                {
                    annualProductionCost.Add(standardAnnualCost*(double) (thinningFactor + custodialFactor));
                    
                }
                else //customdial
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(custodialFactor));
                }
            }



            return annualProductionCost;


        }

        /// <summary>
        /// Method returns yield adjusting for harvests during the project life
        /// </summary>
        /// <returns></returns>
        public IList<double> GetYields(List<double> annualProductivity)
        {
            var rotation = CropAttribute.GetRoationYears(General.Category);
            //for annual crops return the original productivity
            if (rotation == 1)
            {
                return annualProductivity; 
            }
            
            
            if (General.ProjectLife <= rotation)
            {
                return annualProductivity;

            }

            //get the lenght of harvst cycle and repeat
            var cycleYield = annualProductivity.Take(rotation).ToList();

            annualProductivity.Clear();
            var projectLife = General.ProjectLife ?? 10;

            for (int x = 0; x < projectLife; x= x+rotation)
            {
                var trim = x + cycleYield.Count < projectLife ? cycleYield.Count : projectLife - x;
                annualProductivity.AddRange(cycleYield.Take(trim));
            }

            return annualProductivity;
        }
        /// <summary>
        /// The method returns an array of annual productivity
        /// Uses taper function that makes the annual productivity go form
        /// initial value to matured stand.  The taper function is made sepcific 
        /// by croptype. 
        /// </summary>
        /// <returns></returns>
        public IList<double> GetAnnualProductionList()
        {

            

            var taper = CropAttribute.GetProductivityTaper(General.Category);
            var annualProductivity = new List<double>();
            double storageLossFactor = 0;
            if (Storage != null && Storage.RequireStorage != null && (bool)Storage.RequireStorage)
                storageLossFactor = GetStorageLossFactor() * Storage.PercentStored / 100;

            var standardAnnualProduction = GetAnnualProductivity() * (decimal) (1 - storageLossFactor); //Annual Productivity is = Pruduction * (1 - loss factor)
            for (var i = 0; i < General.ProjectLife; i++)
            {
                if (i < taper.Count)
                {
                    var taperValue = taper.ElementAt(i);
                    double delta = (double) (standardAnnualProduction * (decimal) taperValue);
                    annualProductivity.Add(delta);
                }
                else
                {
                    annualProductivity.Add((double) standardAnnualProduction);
                }
            }

            return GetYields(annualProductivity);
        }



        public IList<double> GetGrossProductionList()
        {
            var taper = CropAttribute.GetProductivityTaper(General.Category);
            var annualProductivity = new List<double>();
            double standardAnnualProduction = (double) GetAnnualProductivity(); //Annual Productivity is = Pruduction * (1 - loss factor)
            for (var i = 0; i < General.ProjectLife; i++)
            {
                if (i < taper.Count)
                {
                    var taperValue = taper.ElementAt(i);
                    var delta = standardAnnualProduction * taperValue;
                    annualProductivity.Add(delta);
                }
                else
                {
                    annualProductivity.Add((double) GetAnnualProductivity());
                }
            }
            return annualProductivity;

        }

        public Double GetStorageLossFactor()
        {
            if (Storage == null)
                return 0;
            var days = Storage.StorageTime;
            if (Math.Abs(days) < 1)
                return 0;
            var storagemethod = Convert.ToInt32(Storage.StorageMethod);
            var storageLossValue = Constants.GetStorageLoss(storagemethod, "Switchgrass");
            return days / 200 * storageLossValue / 100;
        }

	    public object Clone()
	    {
			Input newInput  = (Input)this.MemberwiseClone();
			newInput.General = (General)this.General.Clone();
		    newInput.Financial = (Financial) this.Financial.Clone();
		    newInput.Storage = (Storage) this.Storage.Clone();
			newInput.ProductionCost = (ProductionCostViewModel) this.ProductionCost.Clone();

			return newInput;
		}
    }
}