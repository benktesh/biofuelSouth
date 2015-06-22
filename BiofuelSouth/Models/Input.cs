using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Web.Mvc;
using BiofuelSouth.Services;
using log4net;

namespace BiofuelSouth.Models
{
    public class Input
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public int Id { get; set; }

        public Input()
        {
            General = new General();
            Storage = new Storage();
            Financial = new Financial();
        }

        //[Required]
        //public String State { get; set; }

        //[Required]
        //[DisplayName(@"Name of County")]
        //public string County { get; set; }


        //[Required]
        //[DisplayName(@"Biofuel Category")]
        //public String Category { get; set; }

        //[DisplayName(@"Size of Project (acre)")]
        //public double ProjectSize {get; set;}

        //[DisplayName(@"Years (From plantation to harvest")]
        //public int ProjectLife {get; set;}  //years

        //[DisplayName(@"Farm Gate Price ($/dry ton)")]
        //public double BiomassPriceAtFarmGate { get; set; } //$/ton


        //[DisplayName(@"Cost of land ($/acre/year)")]
        //public double LandCost { get; set; } //$/acre/year

        //public bool? ModelStorage { get; set; }
        //public bool? ModelFinancial { get; set; }

        public General General { get; set; }

        public Storage Storage { get; set; }

        public Financial Financial { get; set; }

        //TODO Move everythign to resultmanagement

        /*
         * 
         * Cashflow model:
         * Row[0]
         * Row[1]
         * Row[2]
         * */

        public double[] GetCashFlow()
        {
            var duration = General.ProjectLife;
            var cashFlow = new double[duration];

            //for each year
            //estimate expesese
            var expenses = GetExpenditures();
            var revenues = GetRevenues();
           
            //estimate reveneue
            //get net and insert into cashflow
            for (var i = 0; i < duration; i++)
            {
                cashFlow[i] = -expenses[i].TotalExpenses + revenues[i].TotalRevenue;
            }
            
            return cashFlow;
        }

        public Double GetNPV()
        {
            var cashFlow = GetCashFlow();
            var npv = Microsoft.VisualBasic.Financial.NPV(Financial.InterestRate, ref cashFlow);
            return npv;
        }
       
        public  List<Expenditure> GetExpenditures()
        {
            var expenses = new List<Expenditure>();
            try
            {
                var duration = General.ProjectLife;
                for (var i = 0; i < duration; i++)
                {
                    var expenditure = new Expenditure();
                    expenditure.Year = i;
                    expenditure.AdministrativeCost = Financial.AdministrativeCost;
                    expenditure.LandCost = General.LandCost;
                    expenditure.ProductionCost = GetCostPerAcre();
                    expenditure.TotalExpenses = expenditure.AdministrativeCost + expenditure.LandCost + expenditure.ProductionCost;
                    expenditure.TotalExpenses = expenditure.TotalExpenses*General.ProjectSize;
                    expenses.Add(expenditure);

                    //Add interests
                }
            }
            catch (Exception exception)
            {
                Log.Error("Expenses cannot be calculated");
            }
            return expenses;
        }

        public IList<Revenue> GetRevenues()
        {
            var revenues = new List<Revenue>();
            try
            {
                var production = GetAnnualProductionList();
                var duration = General.ProjectLife;
                for (var i = 0; i < duration; i++)
                {
                    var revenue = new Revenue();
                    revenue.Year = i;
                    revenue.IncentivePayments = 0; 
                    //if the current year is less than the years of incenptive payments, then there is a incentivepayment revenue;
                    if (i < Financial.YearsOfIncentivePayment)
                    {
                        revenue.IncentivePayments = Financial.IncentivePayment;
                    }
                    revenue.BiomassPrice = production[i]; 
                    revenue.TotalRevenue = (revenue.IncentivePayments + revenue.BiomassPrice)*General.ProjectSize;

                    revenues.Add(revenue);
                }
            }
            catch (Exception exception)
            {
                Log.Error("Revenues cannot be calcualted");
            }
            return revenues;
        } 

        /// <summary>
        /// Returns total annual production for the project size;
        /// </summary>
        /// <returns></returns>
        public double GetAnnualProductivity()
        {
            return DataService.GetProductivityPerAcreForCropByGeoId(General.Category, General.County)*General.ProjectSize;  
        }

        public double GetCostPerAcre()
        {
            return (DataService.GetCostPerAcreForCropByGeoId(General.Category, General.County));
        }

        public double GetAnnualCost()
        {
            return (DataService.GetCostPerAcreForCropByGeoId(General.Category, General.County) + General.LandCost) * General.ProjectSize;  
        }

        public double GetBiomassPrice()
        {
            if (Convert.ToInt32(General.BiomassPriceAtFarmGate) == 0)
            {
                General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(General.Category);
            }
            return General.BiomassPriceAtFarmGate;
        }

        public double GetAnnualRevenue()
        {
            if (Convert.ToInt32(General.BiomassPriceAtFarmGate) == 0)
            {
                General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(General.Category); 
            }
            return GetAnnualProductivity() * General.BiomassPriceAtFarmGate;  
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
            var annualProductivity = new List<double>();
            double storageLossFactor = 0;
            if (Storage != null && Storage.RequireStorage != null && (bool) Storage.RequireStorage)
                storageLossFactor = GetStorageLossFactor()*Storage.PercentStored/100;

            var StandardAnnualProduction = GetAnnualProductivity()*(1 - storageLossFactor); //Annual Productivity is = Pruduction * (1 - loss factor)
            for (var i = 0; i < General.ProjectLife; i++)
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
            var annualProductivity = new List<double>();
             double StandardAnnualProduction = GetAnnualProductivity(); //Annual Productivity is = Pruduction * (1 - loss factor)
             for (var i = 0; i < General.ProjectLife; i++)
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
            var days = Storage.StorageTime;
            if (days == 0.0)
                return 0; 
            var storagemethod = Convert.ToInt32(Storage.StorageMethod);
            var storageLossValue = Constants.GetStorageLoss(storagemethod, "Switchgrass");
            return days/200*storageLossValue/100;
        }

    }
}