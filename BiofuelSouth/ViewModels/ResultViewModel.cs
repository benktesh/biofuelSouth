using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using BiofuelSouth.Enum;

namespace BiofuelSouth.ViewModels
{
    public class ResultViewModel
    {

        public ResultViewModel()
        {
            ProductionList = new List<double> { 0 };
            RevenueList = new List<double> { 0 };
            CostList = new List<double> { 0 };
            StorageCostList = new List<double> { 0 };
            LandCotList = new List<double> { 0 };
            ProductionCostList = new List<double> { 0 };
            CashFlow = new List<decimal> { 0 };

        }

        #region inputs

        public CropType CropType { get; set; }
        public string CountyName { get; set; }

        public string StateCode { get; set; }

        public string StateName { get; set; }

        public int ProjectLife { get; set; }

        public double StorageLossFactor { get; set;  }
        
        public double StoragePercent { get; set; }

        public double StorageTime { get; set;  } //days




        #endregion

        #region Summary
        public string NPV { get; set; }
        public string AnnualProduction { get { return ProductionList.Average().ToString("###,###.##"); } }
        public string AnnualCost { get { return CostList.Average().ToString("C0"); } }

        public string AnnualRevenue => RevenueList.Average().ToString("C0");

        public string AverageCostPerAcre { get; set; }
        public string AverageProdutivityPerAcre { get; set; }

        public bool RequireStorage { get; set; }

        #endregion

        #region Assumption
        public decimal BiomassPriceAtFarmGate { get; set; }
        public double ProjectSize { get; set; }
        public double LandCost { get; set; }
        public double AverageAnnualCost { get; set; }
        public double AverageAnnualYield { get; set; }
        #endregion

        #region dss-result
        public List<double> ProductionList { get; set; }
        public List<double> RevenueList { get; set; }
        public List<double> CostList { get; set; }
        public List<double> StorageCostList { get; set; }
        public List<double> LandCotList { get; set; }
        public List<double> ProductionCostList { get; set; }
        public List<decimal> CashFlow { get; set; }
        #endregion
    }
}