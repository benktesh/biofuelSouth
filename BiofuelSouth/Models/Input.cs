using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
       
        public  List<Expenditure> GetExpenditures()
        {
            var expenses = new List<Expenditure>();
            var storageCost = Constants.GetStorageCost(this);

            try
            {
                var duration = General.ProjectLife;
                for (var i = 0; i < duration; i++)
                {
                    var expenditure = new Expenditure();
                    expenditure.Year = i;
                    expenditure.AdministrativeCost = Financial.AdministrativeCost;
                    expenditure.LandCost = General.LandCost.GetValueOrDefault();
                    expenditure.ProductionCost = GetCostPerAcre();
                    if (storageCost != null)
                    {
                        expenditure.StorageCost = storageCost[i];
                    }
                    else
                    {
                        expenditure.StorageCost = 0; 
                    }
                    
                    expenditure.TotalExpenses = expenditure.AdministrativeCost + expenditure.LandCost + expenditure.ProductionCost;
                    expenditure.TotalExpenses = expenditure.TotalExpenses * General.ProjectSize.GetValueOrDefault() + expenditure.StorageCost
                        + Financial.LoanAmount*(Financial.EquityLoanInterestRate/100);
                    expenses.Add(expenditure);

                    //Add interests
                }
            }
            catch (Exception)
            {
                Log.Error("Expenses cannot be calculated");
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
        /// Returns total annual production for the project size;
        /// </summary>
        /// <returns></returns>
        public double GetAnnualProductivity()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(General.Category, General.County)*General.ProjectSize.GetValueOrDefault();  
        }

        public double GetCostPerAcre()
        {
            if (ProductionCost.TotalProductionCost > 0)
                return (double) ProductionCost.TotalProductionCost;
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
        /// <summary>
        /// The method returns an array of annual productivity
        /// Uses taper function that makes the annual productivity go form
        /// initial value to matured stand.  The taper function is made sepcific 
        /// by croptype. 
        /// </summary>
        /// <returns></returns>

        public IList<double> GetAnnualProductionList()
        {
            var taper = Constants.GetProductivityTaper("Switchgrass");
            var annualProductivity = new List<double>();
            double storageLossFactor = 0;
            if (Storage != null && Storage.RequireStorage != null && (bool) Storage.RequireStorage)
                storageLossFactor = GetStorageLossFactor()*Storage.PercentStored/100;

            var standardAnnualProduction = GetAnnualProductivity()*(1 - storageLossFactor); //Annual Productivity is = Pruduction * (1 - loss factor)
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
            return annualProductivity;
        }



        public IList<double> GetGrossProductionList()
        {
            var taper = Constants.GetProductivityTaper("Switchgrass");
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
            return days/200*storageLossValue/100;
        }

    }
}