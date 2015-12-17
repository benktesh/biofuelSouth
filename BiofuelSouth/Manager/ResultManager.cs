using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
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

        private General General { get;  }

        private Storage Storage { get;  } 
        private Financial Financial { get;  }
        private ProductionCostViewModel ProductionCost { get;  } 

        public ResultManager(Input input)
        {
            Input = Input;
            General = Input.General;
            Storage = Input.Storage;
            Financial = Input.Financial;
            ProductionCost = Input.ProductionCost; 
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


        public string BiomassPriceAtFarmGate => $"{General.BiomassPriceAtFarmGate.GetValueOrDefault().ToString("C0")} per ton";

        public string ProjectSize => $"{General.ProjectSize.GetValueOrDefault().ToString("##,###")} Acre";

        public string LandCost => $"{General.LandCost.GetValueOrDefault().ToString("C0")} per Acre";

        public ResultViewModel GetResultViewModel()
        {
            var vm = new ResultViewModel();
            vm.CountyName = Constants.CountyName(General.County);
            vm.CropType = General.Category;
            vm.StateCode = General.State;
            vm.RequireStorage = Storage.RequireStorage.GetValueOrDefault();
           
            return vm;
        }
    }
}