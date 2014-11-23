using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

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
        public double ProjectSize {get; set;}

        public double InterestRate { get; set; }
                
        public int ProjectLife {get; set;}  //years
        public double LandCost {get; set;} //$/acre/year
        public double AdministrativeCost {get; set;} //$/acre/year
        public double IncentivePayment {get; set;} //$/acre/year
        public int YearsOfIncentivePayment {get; set; } //$acres/year
        public double BiomassPriceAtFarmGate {get; set;} //$/ton

        public double AvailableEquity {get; set;} //$
        public double LoanAmount {get; set;} //$
        public double EquityLoanInterestRate {get; set;} //% (decimal fraction)
    }
}