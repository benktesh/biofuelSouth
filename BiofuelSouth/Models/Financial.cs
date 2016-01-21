using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BiofuelSouth.Enum;

namespace BiofuelSouth.Models
{
    public class Financial
    {
        public WizardStep CurrentStep { get; set; }

        [DisplayName(@"Require Finance (Select No to Skip)")]
        [Required]
        public bool? RequireFinance { get; set; }

        [DisplayName(@"Interest Rate(%)")]
        [Required]
        public double InterestRate { get; set; }


        [DisplayName(@"Administrative Cost($/acre/year)")]
        [Required]
        public decimal AdministrativeCost { get; set; } //$/acre/year

        [DisplayName(@"Incentive Payment($/acre/year)")]
        [Required]
        public decimal IncentivePayment { get; set; } //$/acre/year

        [DisplayName(@"No. of Years of Payment")]
        [Required]
        public int YearsOfIncentivePayment { get; set; } //$acres/yearC:\Users\Benktesh\Documents\Visual Studio 2013\Projects\BiofuelSouthSolution\BiofuelSouth\Models\Input.cs

        [DisplayName(@"Available Equity ($)")]
        [Required]
        public double AvailableEquity { get; set; } //$

        [DisplayName(@"Loan Amount ($)")]
        [Required]
        public decimal LoanAmount { get; set; } //$

        [DisplayName(@"Equity Loan Interest Rate (%)")]
        [Required]
        public double EquityLoanInterestRate { get; set; } //% (decimal fraction)

        public string PreviousAction { get; set; }
    }
}