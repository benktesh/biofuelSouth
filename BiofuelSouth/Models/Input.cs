using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using BiofuelSouth.Services;

namespace BiofuelSouth.Models
{
    public class Input
    {

        public int Id { get; set; }

        [Required]
        public String State { get; set; }

        [Required]
        [DisplayName(@"Name of County")]
        public string County { get; set; }


        [Required]
        [DisplayName(@"Biofuel Category")]
        public String Category { get; set; }

        [DisplayName(@"Size of Project (acre)")]
        public double ProjectSize {get; set;}

        [DisplayName(@"Years (From plantation to harvest")]
        public int ProjectLife {get; set;}  //years

        [DisplayName(@"Farm Gate Price ($/dry ton)")]
        public double BiomassPriceAtFarmGate { get; set; } //$/ton


        [DisplayName(@"Cost of land ($/acre/year)")]
        public double LandCost { get; set; } //$/acre/year

        public bool? ModelStorage { get; set; }
        public bool? ModelFinancial { get; set; }

        public General General { get; set; }

        public Storage Storage { get; set; }

        public Financial Finance { get; set; }

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
            if (Storage != null && Storage.RequireStorage)
                storageLossFactor = GetStorageLossFactor()*Storage.PercentStored/100;

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
            if (Storage == null)
                return 0;
            Double days = Storage.StorageTime;
            if (days == 0.0)
                return 0; 
            int storagemethod = Convert.ToInt32(Storage.StorageMethod);
            double storageLossValue = Constants.GetStorageLoss(storagemethod, "Switchgrass");
            return days/200*storageLossValue/100;
        }

    }
}