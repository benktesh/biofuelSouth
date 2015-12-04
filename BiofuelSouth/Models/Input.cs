using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BiofuelSouth.Enum;
using BiofuelSouth.Services;
using log4net;
using LogManager = log4net.LogManager;

namespace BiofuelSouth.Models
{
    public class Input
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int Id { get; set; }

        public Input()
        {
            General = new General();
            Storage = new Storage();
            Financial = new Financial();
            ProductionCost = new ProductionCostViewModel();
        }


        public General General { get; set; }

        public Storage Storage { get; set; }

        public Financial Financial { get; set; }

        public ProductionCostViewModel ProductionCost { get; set; }



        public double[] GetCashFlow()
        {
            var duration = General.ProjectLife;
            var cashFlow = new double[duration.GetValueOrDefault()];

            //for each year
            //estimate expesese
            var expenses = GetExpenditures();
            var revenues = GetRevenues();

            //estimate reveneue
            //get net and insert into cashflow
            for (var i = 0; i < duration; i++)
            {
                cashFlow[i] = -expenses[i].TotalExpenses + revenues[i].TotalRevenue;
            }

            return cashFlow;
        }

        public Double GetNpv()
        {
            var cashFlow = GetCashFlow();
            var npv = Microsoft.VisualBasic.Financial.NPV(Financial.InterestRate, ref cashFlow);
            return npv;
        }

        public List<Expenditure> GetExpenditures()
        {
            var expenses = new List<Expenditure>();
            var storageCost = Constants.GetStorageCost(this);

            try
            {
                var rotation = CropAttribute.GetRoationYears(General.Category);
                var duration = General.ProjectLife ?? 10;
                var annualProductionCosts = GetAnnualProductionCosts();
                for (var i = 0; i < duration; i++)
                {
                    var expenditure = new Expenditure
                    {
                        Year = i,
                        AdministrativeCost = Financial.AdministrativeCost,
                        LandCost = General.LandCost.GetValueOrDefault(),
                        ProductionCost = annualProductionCosts[i]
                    };

                    expenditure.StorageCost = storageCost != null ? storageCost[i] : 0;

                    expenditure.TotalExpenses = expenditure.AdministrativeCost + expenditure.LandCost +
                                                expenditure.ProductionCost;
                    expenditure.TotalExpenses = expenditure.TotalExpenses*General.ProjectSize.GetValueOrDefault() +
                                                expenditure.StorageCost
                                                + Financial.LoanAmount*(Financial.EquityLoanInterestRate/100);
                    expenses.Add(expenditure);

                    //Add interests
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
                        revenue.IncentivePayments = Financial.IncentivePayment * General.ProjectSize.GetValueOrDefault();
                    }
                    revenue.BiomassPrice = production[i] * GetBiomassPrice();
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
        public double GetAnnualProductivity()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(General.Category, General.County) * General.ProjectSize.GetValueOrDefault();
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
                return ((double)(ProductionCost.TotalProductionCost) + General.LandCost.GetValueOrDefault()) *
                    General.ProjectSize.GetValueOrDefault();
            return (DataService.GetCostPerAcreForCropByGeoId(General.Category, General.County) + General.LandCost.GetValueOrDefault()) * General.ProjectSize.GetValueOrDefault();
        }

        public double GetBiomassPrice()
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
            return GetAnnualProductivity() * General.BiomassPriceAtFarmGate.GetValueOrDefault();
        }


        public IList<double> GetAnnualProductionCosts()
        {
            var rotation = CropAttribute.GetRoationYears(General.Category);
            var duration = General.ProjectLife ?? 10;


            var annualProductionCost = new List<double>();

            var standardAnnualCost = GetCostPerAcre();

            if (rotation == 1)
            {
                annualProductionCost.Select(c => { c = standardAnnualCost; return c; }).ToList();
            }

            decimal SitePlantationFactor = 0.38M;
            decimal ThinningFactor = 0.10M;
            decimal HarvestFactor = 0.50M;
            decimal CustodialFactor = 0.02M;


            if (ProductionCost.UseCustom && ProductionCost.ProductionCosts.Any())
            {
                var total = ProductionCost.TotalProductionCost;
                var o3 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.Planting ||
                        m.ProductionCostType == ProductionCostType.SitePreparation).Sum(m => m.Amount) / total;
                if (o3 != null)
                    SitePlantationFactor =
                        (decimal)o3;

                var o2 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.CustodialManagement).Sum(m => m.Amount) / total;
                if (o2 !=
                    null)
                    ThinningFactor =
                        (decimal)o2;

                var o = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.Harvesting).Sum(m => m.Amount) / total;
                if (o != null)
                    HarvestFactor =
                        (decimal)o;

                var o1 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.CustodialManagement).Sum(m => m.Amount) / total;
                if (o1 !=
                    null)
                    CustodialFactor =
                        (decimal)o1;
                Log.Info("Custom cost included");
            }

            //user factor
            //.38 for site preparatio and planting
            //.10 for thinnin
            //.50 for harvesting
            //.02 custodial management

            var thinningYear = (int)Math.Ceiling(rotation / 2.0);

            for (var i = 0; i < General.ProjectLife; i++)
            {
                if (i % rotation == 0)
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(SitePlantationFactor + CustodialFactor));
                }

                if (i > 0 && (i + 1) % rotation == 0)
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(HarvestFactor + CustodialFactor));
                }

                if (i > 0 && (i + 1) % thinningYear == 0)
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(ThinningFactor + CustodialFactor));
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
            if (General.ProjectLife <= rotation)
            {
                return annualProductivity;

            }

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

            var standardAnnualProduction = GetAnnualProductivity() * (1 - storageLossFactor); //Annual Productivity is = Pruduction * (1 - loss factor)
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
                    annualProductivity.Add(standardAnnualProduction);
                }
            }

            return GetYields(annualProductivity);
        }



        public IList<double> GetGrossProductionList()
        {
            var taper = CropAttribute.GetProductivityTaper(General.Category);
            var annualProductivity = new List<double>();
            double standardAnnualProduction = GetAnnualProductivity(); //Annual Productivity is = Pruduction * (1 - loss factor)
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
                    annualProductivity.Add(GetAnnualProductivity());
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

    }
}