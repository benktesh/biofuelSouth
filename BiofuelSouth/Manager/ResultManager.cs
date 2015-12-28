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

        private ResultViewModel vm { get; set; }


        private IList<Revenue> Revenues { get; set; }
        private decimal BiomassPriceAtFarmGate { get; set; }

    public ResultManager(Input input)
        {
            Input = Input;
            General = Input.General;
            Storage = Input.Storage;
            Financial = Input.Financial;
            ProductionCost = Input.ProductionCost; 

            vm = new ResultViewModel();
        // Set properties for calculations

        vm.BiomassPriceAtFarmGate = BiomassPriceAtFarmGate;
        vm.CashFlow = GetCashFlow();

        //set all view model properties
        }

        public ResultViewModel GetResult()
        {
            vm.CashFlow = GetCashFlow();

            vm.BiomassPriceAtFarmGate = $"{BiomassPriceAtFarmGate.ToString("C0")} per ton";
            vm.ProjectSize = $"{General.ProjectSize.GetValueOrDefault().ToString("##,###")} Acre";
            vm.LandCost = $"{General.LandCost.GetValueOrDefault().ToString("C0")} per Acre";
            return vm;
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
        private List<decimal> GetCashFlow()
        {
            var duration = General.ProjectLife;
            var cashFlow = new List<decimal>();  
            //for each year
            //estimate expesese
            var expenses = GetExpenditures();
            var revenues = GetRevenues();

            //estimate reveneue
            //get net and insert into cashflow
            for (var i = 0; i < duration; i++)
            {
               var calc = -expenses[i].TotalExpenses + revenues[i].TotalRevenue;
                cashFlow.Add((decimal) calc);
            }
            return cashFlow;

        }
        private List<Revenue> GetRevenue()
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
                        ProductionCost = annualProductionCosts[i]
                    };

                    expenditure.StorageCost = storageCost != null && storageCost.Any() ? storageCost[i] : 0;

                    expenditure.TotalExpenses = expenditure.AdministrativeCost + expenditure.LandCost +
                                                expenditure.ProductionCost;
                    expenditure.TotalExpenses = expenditure.TotalExpenses * General.ProjectSize.GetValueOrDefault() +
                                                expenditure.StorageCost
                                                + Financial.LoanAmount * (Financial.EquityLoanInterestRate / 100);
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


       

        public string ProjectSize => $"{General.ProjectSize.GetValueOrDefault().ToString("##,###")} Acre";

        public string LandCost => $"{General.LandCost.GetValueOrDefault().ToString("C0")} per Acre";

        public ResultViewModel GetResultViewModel()
        {
            var vm = new ResultViewModel();
            vm.CountyName = Constants.CountyName(General.County);
            vm.CropType = General.Category;
            vm.StateCode = General.State;
            vm.RequireStorage = Storage.RequireStorage.GetValueOrDefault();
           

            //populate values.
            //make chart

            return vm;
        }

        public void PrepareChart(ResultViewModel vm)
        {
            //make cachekey and load to viewmodel
            var c = new ChartController();
            var revenueCachekey = Guid.NewGuid().ToString();

            vm.ChartKeys.Add(ChartType.Revenue, revenueCachekey);
            
            //c.GenerateCostRevenueChart(costRevenueCachekey, ip.GetRevenues().Select(m => m.TotalRevenue).ToArray(), "Cost and Revenue");
            var rev = ip.GetRevenues().Select(m => m.TotalRevenue).ToArray();
            c.GenerateChart(revenueCachekey, rev, "Revenue");

            var AnnualProd = ip.GetAnnualProductionList().ToArray();
            var cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey = cacheKey;
            var cc = new ChartController();
            cc.GenerateChart(cacheKey, AnnualProd, "Annual Production");

            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey1 = cacheKey;
            cc.GenerateCostRevenueChart(cacheKey, ip, "Cost and Revenue");

            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey3 = cacheKey;
            cc.GenerateColumnChart(cacheKey, ip.GetCashFlow(), "Cash Flow", "Year ", "$");

        }
    }
}