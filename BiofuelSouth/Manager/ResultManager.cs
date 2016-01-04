using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using BiofuelSouth.Controllers;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.Services;
using BiofuelSouth.ViewModels;
using log4net;

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

        private IList<decimal> Productivity { get; set;  } 

        private IList<decimal> GrossProductions { get; set;  }

        private List<decimal> StorageLoss { get; set; } 

        private ResultViewModel vm { get; set; }

        private IList<Revenue> Revenues { get; set; }

        private IList<Expenditure> Expenses { get; set; }

        private decimal BiomassPriceAtFarmGate { get; set; }

        private decimal NPV { get; set;  }

        public ResultManager(Input input)
        {
            Input = input;
            General = input.General;

            if (General.ProjectLife == null)
            {
                General.ProjectLife = 10; 
            }
            Storage = input.Storage;
            Financial = input.Financial;
            ProductionCost = input.ProductionCost; 

            vm = new ResultViewModel();
        // Set properties for calculations

            BiomassPriceAtFarmGate = GetBiomassPrice();
            GrossProductions = GetGrossProductionList();
            Productivity = GetAnnualProductionList(true); 

            Productions = GetAnnualProductionList();
            Revenues = GetRevenues();
            Expenses = GetExpenditures(); 
            StorageLoss = GetStorageLossList(); 

            NPV = GetNpv();

            //set all view model properties
        }

        public Decimal GetNpv()
        {
            var cashFlow = GetCashFlow().Select(m=>(double) m).ToArray(); 
            var npv = (decimal) Microsoft.VisualBasic.Financial.NPV(Financial.InterestRate, ref cashFlow);
            return npv;
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

        private List<Decimal> GetStorageLossList()
        {
            if (Productions == null || Productions.Count == 0)
                return null;

            return Productions.Select(m => m * (decimal)(GetStorageLossFactor()* Storage.StorageTime/200*Storage.PercentStored/100)).ToList();
        
         }
        private List<decimal> GetYields(List<decimal> annualProductivity, bool isProductivity = false)
        {
            var rotation = CropAttribute.GetRoationYears(General.Category);
            //for annual crops return the original productivity
            if (rotation == 1)
            {
                return annualProductivity;
            }


            if (General.ProjectLife <= rotation)
            {
                if (isProductivity) // do not send cummulative
                {
                    return annualProductivity;
                }
               return MakeCumulative(annualProductivity);
            }

            //get the lenght of harvst cycle and repeat
            var cycleYield = annualProductivity.Take(rotation).ToList();

            annualProductivity.Clear();
            var projectLife = General.ProjectLife ?? 10;

            for (int x = 0; x < projectLife; x = x + rotation)
            {
                var trim = x + cycleYield.Count < projectLife ? cycleYield.Count : projectLife - x;
                if (isProductivity) // do not send cummulative
                {
                    annualProductivity.AddRange(cycleYield.Take(trim).ToList());
                }
                else
                {
                    if (trim == rotation)
                    {
                        //for rotation, cumulatively add the net annual
                        annualProductivity.AddRange(MakeCumulative(cycleYield.Take(trim).ToList()));

                    }
                    else
                    {
                        //if this cycle is less than rotation, then set yeild to 0
                        annualProductivity.AddRange(cycleYield.Take(trim).Select(m => 0m).ToList());
                    }
                }
            }

            return annualProductivity;
        }

        private List<decimal> MakeCumulative(List<decimal> annual)
        {
            var finalYield = annual.Sum();
            var annualCopy = annual.Select(m => 0m).ToList();
            annualCopy[annual.Count-1] = finalYield;
            return annualCopy; 
        } 

        private List<decimal> GetAnnualProductionList( bool isProductivity = false)
        {
            var taper = CropAttribute.GetProductivityTaper(General.Category);
            var annualProductivity = new List<decimal>();
            double storageLossFactor = 0;
            if (Storage != null && Storage.RequireStorage != null && (bool)Storage.RequireStorage)
                storageLossFactor = GetStorageLossFactor() * Storage.PercentStored / 100;

            var standardAnnualProduction = (decimal) (GetAnnualProductivity() * (1 - storageLossFactor)); 
            //Annual Productivity is = Pruduction * (1 - loss factor)
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
            return GetYields(annualProductivity, isProductivity);
        }

        public IList<decimal> GetGrossProductionList()
        {
            var taper = CropAttribute.GetProductivityTaper(General.Category).Select(m=>(Decimal)m).ToList();
            var annualProductivity = new List<decimal>();
            decimal standardAnnualProduction = (decimal) GetAnnualProductivity(); //Annual Productivity is = Pruduction * (1 - loss factor)
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
                    annualProductivity.Add((decimal)GetAnnualProductivity());
                }
            }
            return annualProductivity;

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
            return General.BiomassPriceAtFarmGate.GetValueOrDefault();
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
                    revenue.BiomassPrice = Productions[i] * BiomassPriceAtFarmGate;
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
            
            for (var i = 0; i < duration; i++)
            {
               var calc = -Expenses[i].TotalExpenses + Revenues[i].TotalRevenue;
                cashFlow.Add(calc);
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
                var duration = General.ProjectLife ?? Constants.ProjectLife;
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

                    expenditure.StorageCost = storageCost != null && storageCost.Any() ? storageCost[i] : 0;

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
                        m.ProductionCostType == ProductionCostType.SitePreparation).Sum(m => m.Amount) / total;
                if (o3 != null)
                    sitePlantationFactor =
                        (decimal)o3;

                var o2 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.CustodialManagement).Sum(m => m.Amount) / total;
                if (o2 !=
                    null)
                    thinningFactor =
                        (decimal)o2;

                var o = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.Harvesting).Sum(m => m.Amount) / total;
                if (o != null)
                    harvestFactor =
                        (decimal)o;

                var o1 = ProductionCost.ProductionCosts.Where(
                    m =>
                        m.ProductionCostType == ProductionCostType.CustodialManagement).Sum(m => m.Amount) / total;
                if (o1 !=
                    null)
                    custodialFactor =
                        (decimal)o1;
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

                else if (thinningYear != null && (i > 0 && ((i + 1) % thinningYear == 0) || ((i % rotation) + 1) % thinningYear == 0)) //thinning
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(thinningFactor + custodialFactor));

                }
                else //customdial
                {
                    annualProductionCost.Add(standardAnnualCost * (double)(custodialFactor));
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
            vm.ProjectLife = General.ProjectLife.GetValueOrDefault();
            vm.NPV = $"{NPV.ToString("C")}";

            vm.ProductionList = Productions;
            vm.GrossProductionList = GrossProductions;
            vm.ProductivityList = Productivity; 

            vm.RevenueList = Revenues.Select(m=>m.TotalRevenue).ToList();
            vm.CostList = Expenses.Select(m => m.TotalExpenses).ToList();

            vm.StorageLoss = StorageLoss;
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
            vm.Add(ChartType.CashFlow, cacheKey);
            cc.GenerateColumnChart(cacheKey, GetCashFlow().ToArray(), "Cash Flow", "Year ", "$");

            cacheKey = Guid.NewGuid().ToString();
            cc.GenerateChart(cacheKey, Productions.ToArray(), "Production", "Year", "Yield");
            vm.Add(ChartType.Production, cacheKey);
           
            var revenueCachekey = Guid.NewGuid().ToString();
            vm.Add(ChartType.CostRevenue, revenueCachekey);
            //cc.GenerateCostRevenueChart(cacheKey, ip, "Cost and Revenue");
            var costRevenueData = new List<List<decimal>>(); 
            costRevenueData.Add(Revenues.Select(m=>m.TotalRevenue).ToList());
            costRevenueData.Add(Expenses.Select(m => m.TotalExpenses).ToList());
            cc.GenerateLineGraphs(revenueCachekey,costRevenueData, new List<string> {"Revenue", "Cost"}, "Cost and Revenue" );

            return vm;
        }       
    }

    public class Simulator
    {
        private Input Input { get; set; }

        public Simulator(Input input)
        {
            Input = input;
        }
        private IList<CropType> GetAlternativeCrops()
        {
            var alternative = new HashSet<CropType> { CropType.Switchgrass, CropType.Miscanthus, CropType.Willow, CropType.Poplar, CropType.Pine };
            alternative.Remove(Input.General.Category);
            return alternative.ToList();
        }
        public List<ResultViewModel> GetViewModels(bool simulateAlternatives = true)
        {
            var altCrops = GetAlternativeCrops();
            var viewModels = new List<ResultViewModel>();

            var rm = new ResultManager(Input);
            viewModels.Add(rm.GetResultViewModel());

            if (!simulateAlternatives)
                return viewModels;

            foreach (var alt in altCrops)
            {
                Input = GetInput(Input, alt);  
                rm = new ResultManager(Input);
                viewModels.Add(rm.GetResultViewModel());

            }

            return viewModels;

        }

        private Input GetInput(Input ip, CropType alt)
        {
            //Update General
            Input input = ip; 
            input.General.Category = alt;
            input.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(alt);

            ProductionCostManager pcm = new ProductionCostManager();
            input.ProductionCost = pcm.GetProductionCost(new ProductionCostViewModel { CropType = input.General.Category, County = input.General.County, UseCustom = true });

            //Update Storage. For woody, set storage to null
            if (alt == CropType.Poplar || alt == CropType.Pine || alt == CropType.Willow)
            {
                input.Storage = new Storage {RequireStorage = false};
            }


            //Update Financial


            return input;
        }
    }
}