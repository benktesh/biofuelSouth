using System.Security.Cryptography.X509Certificates;

namespace BiofuelSouth.ViewModels
{
    public class ResultsViewModel
    {
        #region Summary
        public decimal NPV { get; set; }
        public decimal AnnualProduction { get; set;  }

        public decimal AnnualCost { get; set; }

        public decimal AnnualRevenue { get; set; }

        #endregion
    }
}