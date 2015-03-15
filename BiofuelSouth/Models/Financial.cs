using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace BiofuelSouth.Models
{
    public class Financial
    {
        [DisplayName(@"Financial Interest Rate(%)")]
        public double InterestRate { get; set; }


        [DisplayName(@"Annual Administrative Cost($/acre/year)")]
        public double AdministrativeCost { get; set; } //$/acre/year

        [DisplayName(@"Incentive Payment($/acre/year)")]
        public double IncentivePayment { get; set; } //$/acre/year

        [DisplayName(@"Years of Incentive Payment (i.e., number of years)")]
        public int YearsOfIncentivePayment { get; set; } //$acres/yearC:\Users\Benktesh\Documents\Visual Studio 2013\Projects\BiofuelSouthSolution\BiofuelSouth\Models\Input.cs



        [DisplayName(@"Available Equity ($)")]
        public double AvailableEquity { get; set; } //$

        [DisplayName(@"Loan Amount ($) (expected or current)")]
        public double LoanAmount { get; set; } //$

        [DisplayName(@"Equity Loan Interest Rate (% e.g. 4.05)")]
        public double EquityLoanInterestRate { get; set; } //% (decimal fraction)
    }
}