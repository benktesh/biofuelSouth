using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using BiofuelSouth.Enum;
using BiofuelSouth.Models.Entity;

namespace BiofuelSouth.ViewModels
{
    public class ResultViewModel
    {

        public ResultViewModel()
        {
            ProductionList = new List<decimal> { 0 };
            RevenueList = new List<decimal> { 0 };
            CostList = new List<decimal> { 0 };
            StorageCostList = new List<decimal> { 0 };
            LandCostList = new List<decimal> { 0 };
            ProductionCostList = new List<decimal> { 0 };
            CashFlow = new List<decimal> { 0 };
            ComparisionData = new List<SummaryComparisionModel>();
        }

        #region inputs

        public CropType CropType { get; set; }
        public string CountyName { get; set;} 

		public CountyEntity CountyEntity { get; set;  }

        public string StateCode { get; set; }

        public string StateName { get; set; }

        [DisplayName(@"Project Duration (Years)")]
        public int ProjectLife { get; set; }


        [DisplayName(@"Storage Loss Factor (%)")]
        public double StorageLossFactor { get; set;  }


        [DisplayName(@"Storage Percent (%)")]
        public double StoragePercent { get; set; }

        [DisplayName(@"Storage Time (days)")]
        public double StorageTime { get; set;  } //days

		[DisplayName( @"Storage Method" )]
		public String StorageMethod { get; set;  }

		[DisplayName( @"Storage Cost Assessment Method" )]
		public String StorageCostmethod { get; set;  }

        [DisplayName(@"Interest Rate (%)")]
        public double InterestRate { get; set; }

		[DisplayName( @"Require Finance (Select No to Skip)" )]
		public bool RequireFinance { get; set; }

		[DisplayName( @"Administrative Cost($/acre/year)" )]
		public decimal AdministrativeCost { get; set; } //$/acre/year

		[DisplayName( @"Incentive Payment($/acre/year)" )]
		public decimal IncentivePayment { get; set; } //$/acre/year

		[DisplayName( @"No. of Years of Payment" )]
		public int YearsOfIncentivePayment { get; set; } //$acres/yearC:\Users\Benktesh\Documents\Visual Studio 2013\Projects\BiofuelSouthSolution\BiofuelSouth\Models\Input.cs

		[DisplayName( @"Available Equity ($)" )]
		public double AvailableEquity { get; set; } //$

		[DisplayName( @"Loan Amount ($)" )]
		public decimal LoanAmount { get; set; } //$

		[DisplayName( @"Equity Loan Interest Rate (%)" )]
		public double EquityLoanInterestRate { get; set; } //% (decimal fraction)




		#endregion

		#region Summary
		public decimal NPV { get; set; }

        [DisplayName(@"Annual Production (tons)")]
        public string AnnualProduction { get { return ProductionList.Average().ToString("###,###.##"); } }
        public string AnnualCost { get { return CostList.Average().ToString("C0"); } }

        public string AnnualRevenue => RevenueList.Average().ToString("C0");

		[DisplayName( @"Average Cost ($/per ton)")]
		public string AverageCostPerAcre { get; set; }

		[DisplayName( @"Average Production (ton/acre/year)" )]
		public string AverageProdutivityPerAcre { get; set; }

		[DisplayName( @"Storage" )]
		public bool RequireStorage { get; set; }

		public Tuple<string, string, string> ImageUrl { get; set; }

        #endregion

        #region Assumption
        public decimal? BiomassPriceAtFarmGate { get; set; }
        [DisplayName(@"Project Size (Acres)")]
        public string ProjectSize { get; set; }

		[DisplayName( @"Land Cost ($/acre/year)" )]
		public string LandCost { get; set; }

        public double AverageAnnualCost { get; set; }
        public double AverageAnnualYield { get; set; }
        #endregion

        #region dss-result
        public IList<decimal> GrossProductionList { get; set; }
        public List<decimal> ProductionList { get; set; }

        public IList<decimal> ProductivityList { get; set; } 

        public List<decimal> RevenueList { get; set; }
        public List<decimal> CostList { get; set; }

        public List<decimal> StorageLoss { get; set; } 

        public List<decimal> StorageCostList { get; set; }
        public List<decimal> LandCostList { get; set; }
        public List<decimal> ProductionCostList { get; set; }
        public List<decimal> CashFlow { get; set; }

        public List<SummaryComparisionModel> ComparisionData { get; set;}

        public Dictionary<ChartType, string> ChartKeys { get; set; }  //actualy string of guid is the value 
        #endregion

        public Boolean CanShowPDFLink {
            get
            {
                var key = ConfigurationManager.AppSettings["ShowPDFLink"];
                return (key != null && key.Equals("true", StringComparison.InvariantCultureIgnoreCase));

            }

        }
    }
}