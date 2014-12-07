using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using BiofuelSouth.Services;

namespace BiofuelSouth.Models
{
    public class Input
    {
        public int Id { get; set; }

        [DisplayName("Biofuel Category")]
        public String Category { get; set; }

        [DisplayName("Name of County")]
        public string County { get; set; }

        public String State { get; set; }
        [DisplayName("Size of Project (acre)")]
        public double ProjectSize {get; set;}

        [DisplayName("Financial Interest Rate(%)")]
        public double InterestRate { get; set; }
        
        [DisplayName("Duration (years from plantation to harvest")]
        public int ProjectLife {get; set;}  //years

        [DisplayName("Cost of land ($/acre/year")]
        public double LandCost {get; set;} //$/acre/year

        [DisplayName("Annual Administrative Cost($/acre/year)")]
        public double AdministrativeCost {get; set;} //$/acre/year

        [DisplayName("Incentive Payment($/acre/year)")]
        public double IncentivePayment {get; set;} //$/acre/year
        
        [DisplayName("Years of Incentive Payment (i.e., number of years)")]
        public int YearsOfIncentivePayment {get; set; } //$acres/yearC:\Users\Benktesh\Documents\Visual Studio 2013\Projects\BiofuelSouthSolution\BiofuelSouth\Models\Input.cs

        [DisplayName("Biomass Price at Farm Gate ($/green ton)")]
        public double BiomassPriceAtFarmGate {get; set;} //$/ton

        [DisplayName("Available Equity ($)")]
        public double AvailableEquity {get; set;} //$

        [DisplayName("Loan Amount ($) (expected or current)")]
        public double LoanAmount {get; set;} //$

        [DisplayName("Equity Loan Interest Rate (% e.g. 4.05)")]
        public double EquityLoanInterestRate {get; set;} //% (decimal fraction)

        public double GetAnnualProductivity()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(this.Category, this.County)*this.ProjectSize;  
        }

        public double GetAnnualCost()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(this.Category, this.County)*this.ProjectSize;  
        }

        public double GetAnnualRevenue()
        {
            if (Convert.ToInt32(BiomassPriceAtFarmGate) == 0)
            {
                this.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(this.Category); 
            }
            return GetAnnualProductivity()*this.BiomassPriceAtFarmGate;  
        }


    }
}