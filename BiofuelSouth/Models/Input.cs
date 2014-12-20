using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Web;
using BiofuelSouth.Services;

namespace BiofuelSouth.Models
{
    public class Input
    {
        public Input()
        {
            ProjectSize = 100;
            ProjectLife = 10;
            Category = "Switchgrass";
            State = "AL";
            County = "01001";
            StorageRequirement = new Storage();
        }
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

        [DisplayName("Cost of land ($/acre/year)")]
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

        public Storage StorageRequirement { get; set; }

        public double GetAnnualProductivity()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(this.Category, this.County)*this.ProjectSize;  
        }

        public double GetAnnualCost()
        {
            return (DataService.GetCostPerAcreForCropByGeoId(this.Category, this.County) + this.LandCost) * this.ProjectSize;  
        }

        public double GetAnnualRevenue()
        {
            if (Convert.ToInt32(BiomassPriceAtFarmGate) == 0)
            {
                this.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(this.Category); 
            }
            return GetAnnualProductivity()*this.BiomassPriceAtFarmGate;  
        }
        /// <summary>
        /// The method returns an array of annual productivity
        /// Uses taper function that makes the annual productivity go form
        /// initial value to matured stand.  The taper function is made sepcific 
        /// by croptype. 
        /// </summary>
        /// <returns></returns>

        public IList<double> GetAnnualProductionList()
        {
            var taper = Constants.GetProductivityTaper("Switchgrass");
            List<double> annualProductivity = new List<double>();
            double storageLossFactor = 0;
            if (StorageRequirement.RequireStorage)
                storageLossFactor = GetStorageLossFactor()*StorageRequirement.PercentStored/100;

            double StandardAnnualProduction = GetAnnualProductivity()*(1 - storageLossFactor); //Annual Productivity is = Pruduction * (1 - loss factor)
            for (int i = 0; i < this.ProjectLife; i++)
            {
                if (i < taper.Count)
                {
                    var taperValue = taper.ElementAt(i);
                    var delta = StandardAnnualProduction * taperValue;
                    annualProductivity.Add(delta);
                }
                else
                {
                    annualProductivity.Add(StandardAnnualProduction);
                }
            }
            return annualProductivity;
        }

        public IList<double> GetGrossProductionList()
        {
            var taper = Constants.GetProductivityTaper("Switchgrass");
            List<double> annualProductivity = new List<double>();
             double StandardAnnualProduction = GetAnnualProductivity(); //Annual Productivity is = Pruduction * (1 - loss factor)
            for (int i = 0; i < this.ProjectLife; i++)
            {
                if (i < taper.Count)
                {
                    var taperValue = taper.ElementAt(i);
                    var delta = StandardAnnualProduction * taperValue;
                    annualProductivity.Add(delta);
                }
                else
                {
                    annualProductivity.Add(GetAnnualProductivity());
                }
            }
            return annualProductivity;
            
        }

        public Double GetStorageLossFactor()
        {
            Double days = this.StorageRequirement.StorageTime;
            if (days == 0.0)
                return 0; 
            int storagemethod = Convert.ToInt32(this.StorageRequirement.StorageMethod);
            double storageLossValue = Constants.GetStorageLoss(storagemethod, "Switchgrass");
            return days/200*storageLossValue/100;
        }



    }
}