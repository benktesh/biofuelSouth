using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BiofuelSouth.Services;

namespace BiofuelSouth.Models
{
    public class Input
    {
        public Input()
        {
            //ProjectSize = 100;
           // ProjectLife = 10;
          //  Category = "Switchgrass";
          //  State = "AL";
          //  County = "01001";
            StorageRequirement = new Storage();
        }
        public int Id { get; set; }


        [Required]
        public String State { get; set; }

        [Required]
        [DisplayName("Name of County")]
        public string County { get; set; }


        [Required]
        [DisplayName("Biofuel Category")]
        public String Category { get; set; }

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
            return DataService.GetProductivityPerAcreForCropByGeoId(Category, County)*ProjectSize;  
        }

        public double GetAnnualCost()
        {
            return (DataService.GetCostPerAcreForCropByGeoId(Category, County) + LandCost) * ProjectSize;  
        }

        public double GetAnnualRevenue()
        {
            if (Convert.ToInt32(BiomassPriceAtFarmGate) == 0)
            {
                BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(Category); 
            }
            return GetAnnualProductivity()*BiomassPriceAtFarmGate;  
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
            for (int i = 0; i < ProjectLife; i++)
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
            for (int i = 0; i < ProjectLife; i++)
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
            Double days = StorageRequirement.StorageTime;
            if (days == 0.0)
                return 0; 
            int storagemethod = Convert.ToInt32(StorageRequirement.StorageMethod);
            double storageLossValue = Constants.GetStorageLoss(storagemethod, "Switchgrass");
            return days/200*storageLossValue/100;
        }



    }
}