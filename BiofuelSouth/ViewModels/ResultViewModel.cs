using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;

namespace BiofuelSouth.ViewModels
{
    public class ResultViewModel
    {
        #region Summary
        public decimal NPV { get; set; }
        public decimal AnnualProduction { get; set; }
        public decimal AnnualCost { get; set; }
        public decimal AnnualRevenue { get; set; }
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
        public List<decimal> CashFlow { get; set;  } 
        #endregion
    }
}