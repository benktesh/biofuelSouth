using System;
using System.Collections.Generic;
using System.EnterpriseServices.Internal;
using System.Linq;
using System.Reflection;
using System.Web;
using BiofuelSouth.Controllers;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.Services;
using BiofuelSouth.ViewModels;
using log4net;
using Microsoft.VisualBasic.Logging;

namespace BiofuelSouth.Manager
{
    public class ResultManager
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private Input Input { get; }

        private General General { get; }

        private Storage Storage { get; }
        private Financial Financial { get; }
        private ProductionCostViewModel ProductionCost { get; }

        private List<decimal> Productions { get; set; }

        private ResultViewModel vm { get; set; }

        private IList<Revenue> Revenues { get; set; }

        private IList<Expenditure> Expenses { get; set; }

        private decimal BiomassPriceAtFarmGate { get; set; }

        public ResultManager(Input input)
        {
            Input = input;
            General = input.General;
            Storage = input.Storage;
            Financial = input.Financial;
            ProductionCost = input.ProductionCost; 

            vm = new ResultViewModel();
        // Set properties for calculations

            BiomassPriceAtFarmGate = GetBiomassPrice();

            Productions = GetAnnualProductionList();
            Revenues = GetRevenues();
            Expenses = GetExpenditures();
            Productions = GetAnnualProductionList();

        //set all view model properties
        }

        private Double GetStorageLossFactor()
        {
            if (Storage == null)
                return 0;
            var days = Storage.StorageTime;
            if (Math.Abs(days) < 1)
                return 0;
            var storagemethod = Convert.ToInt32(Storage.StorageMethod);
            var storageLossValue = Constants.GetStorageLoss(storagemethod, General.Category);
            return days / 200 * (double) storageLossValue / 100;
        }
        private List<decimal> GetYields(List<decimal> annualProductivity)
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

            for (int x = 0; x < projectLife; x = x + rotation)
            {
                var trim = x + cycleYield.Count < projectLife ? cycleYield.Count : projectLife - x;
                annualProductivity.AddRange(cycleYield.Take(trim));
            }

            return annualProductivity;

        }
        private List<decimal> GetAnnualProductionList()
        {

            var taper = CropAttribute.GetProductivityTaper(General.Category);
            var annualProductivity = new List<decimal>();
            double storageLossFactor = 0;
            if (Storage != null && Storage.RequireStorage != null && (bool)Storage.RequireStorage)
                storageLossFactor = GetStorageLossFactor() * Storage.PercentStored / 100;

            var standardAnnualProduction = (decimal) (GetAnnualProductivity() * (1 - storageLossFactor)); //Annual Productivity is = Pruduction * (1 - loss factor)
            for (var i = 0; i < General.ProjectLife; i++)
            {
                if (i < taper.Count)
                {
                    var taperValue = (decimal) taper.ElementAt(i);
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

        /// <summary>
        /// Returns total annual production for the project si;
        /// </summary>
        /// <returns></returns>
        private double GetAnnualProductivity()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(General.Category, General.County) * General.ProjectSize.GetValueOrDefault();
        }
        private decimal GetBiomassPrice()
        {
            if (Convert.ToInt32(General.BiomassPriceAtFarmGate) == 0)
            {
                General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(General.Category);
            }
            return (decimal) General.BiomassPriceAtFarmGate.GetValueOrDefault();
        }

        private IList<Revenue> GetRevenues()
        {
            var revenues = new List<Revenue>();
            try
            {
               
                var duration = General.ProjectLife;
                for (var i = 0; i < duration; i++)
                {
                    var revenue = new Revenue();
                    revenue.Year = i;
                    revenue.IncentivePayments = 0;
                    //if the current year is less than the years of incenptive payments, then there is a incentivepayment revenue;
                    if (i < Financial.YearsOfIncentivePayment)
                    {
                        revenue.IncentivePayments = (Financial.IncentivePayment * (decimal) General.ProjectSize.GetValueOrDefault());
                    }
                    revenue.BiomassPrice = (decimal) Productions[i] * BiomassPriceAtFarmGate;
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
        private List<decimal> GetCashFlow()
        {
            var duration = General.ProjectLife;
            var cashFlow = new List<decimal>();  
            //for each year
            //estimate expesese
            //var expenses = GetExpenditures();
            //var revenues = GetRevenues();

            //estimate reveneue
            //get net and insert into cashflow
            for (var i = 0; i < duration; i++)
            {
               var calc = -Expenses[i].TotalExpenses + Revenues[i].TotalRevenue;
                cashFlow.Add((decimal) calc);
            }
            return cashFlow;

        }
    
        public double GetCostPerAcre()
        {
            if (ProductionCost.TotalProductionCost > 0)
                return (double)ProductionCost.TotalProductionCost;
            return (DataService.GetCostPerAcreForCropByGeoId(General.Category, General.County));
        }

        public List<Expenditure> GetExpenditures()
        {
            var expenses = new List<Expenditure>();
            var storageCost = Constants.GetStorageCost(Input);

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
                        ProductionCost = (decimal) annualProductionCosts[i]
                    };

                    expenditure.StorageCost = (decimal) (storageCost != null && storageCost.Any() ? storageCost[i] : 0);

                    expenditure.TotalExpenses = expenditure.AdministrativeCost + expenditure.LandCost +
                                                expenditure.ProductionCost;
                    expenditure.TotalExpenses = expenditure.TotalExpenses * (decimal)General.ProjectSize.GetValueOrDefault() +
                                                expenditure.StorageCost
                                                + Financial.LoanAmount * ((decimal)Financial.EquityLoanInterestRate / 100);
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

        public IList<double> GetAnnualProductionCosts()
        {
            var rotation = CropAttribute.GetRoationYears(General.Category);
            General.ProjectLife = General.ProjectLife ?? 10;


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

        public ResultViewModel GetResultViewModel()
        {
            vm.CashFlow = GetCashFlow();
     
            vm.BiomassPriceAtFarmGate = $"{BiomassPriceAtFarmGate.ToString("C0")} per ton";
            vm.ProjectSize = $"{General.ProjectSize.GetValueOrDefault().ToString("##,###")} Acre";
            vm.LandCost = $"{General.LandCost.GetValueOrDefault().ToString("C0")} per Acre";

            vm.ProductionList = Productions;
            vm.RevenueList = Revenues.Select(m=>m.TotalRevenue).ToList();
            vm.CostList = Expenses.Select(m => m.TotalExpenses).ToList();
            vm.StorageCostList = Expenses.Select(m => m.StorageCost).ToList(); 

            vm.CountyName = Constants.CountyName(General.County);
            vm.CropType = General.Category;
            vm.StateCode = General.State;
            vm.RequireStorage = Storage.RequireStorage.GetValueOrDefault();

            vm.ChartKeys = PrepareChart();

            //populate values.
            //make chart

            return vm;
        }

        public Dictionary<ChartType, string> PrepareChart()
        {

            Dictionary<ChartType, string> vm = new Dictionary<ChartType, string>();
            //make cachekey and load to viewmodel
            //var c = new ChartController();

            //c.GenerateCostRevenueChart(costRevenueCachekey, ip.GetRevenues().Select(m => m.TotalRevenue).ToArray(), "Cost and Revenue");
            //var rev = Input.GetRevenues().Select(m => m.TotalRevenue).ToArray();
            //  var  rev = Revenues.Select(rv => (double) rv.TotalRevenue).ToArray();
            //c.GenerateChart(revenueCachekey, rev, "Revenue");



            var cc = new ChartController();
  
            var cacheKey = Guid.NewGuid().ToString();
            cc.GenerateChart(cacheKey, Productions.ToArray(), "Annual Production");
            vm.Add(ChartType.Production, cacheKey);

           
            var revenueCachekey = Guid.NewGuid().ToString();
            vm.Add(ChartType.CostRevenue, revenueCachekey);
            //cc.GenerateCostRevenueChart(cacheKey, ip, "Cost and Revenue");
            var costRevenueData = new List<List<decimal>>(); 
            costRevenueData.Add(Revenues.Select(m=>(decimal)m.TotalRevenue).ToList());
            costRevenueData.Add(Expenses.Select(m => (decimal) m.TotalExpenses).ToList());
            cc.GenerateLineGraphs(revenueCachekey,costRevenueData, new List<string> {"Revenue", "Cost"}, "Cost and Revenue" );


            cacheKey = Guid.NewGuid().ToString();
            vm.Add(ChartType.CashFlow, cacheKey);
            cc.GenerateColumnChart(cacheKey, GetCashFlow().ToArray(), "Cash Flow", "Year ", "$");

            return vm;
        }
    }
}