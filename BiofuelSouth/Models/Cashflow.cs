using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace BiofuelSouth.Models
{
    public class Cashflow
    {
        

        public Expenditure _expenditure { get; set; }
        Revenue _revenue { get; set; }
        int Duration { get; set; }
        public Cashflow(Input input)
        {
            
            Duration = input.General.ProjectLife.GetValueOrDefault();

            _expenditure.AdministrativeCost = input.Financial.AdministrativeCost;
            _expenditure.LandCost = input.General.LandCost.GetValueOrDefault();
            _expenditure.PlantingAndEstablishmentCost = 0;  
            _expenditure.StandMaintenanceCost = 0;
            _expenditure.HarvestCost = 0;
            _expenditure.StorageCost = 0; 
            _expenditure.TransportationCost = 0;

            _revenue.BiomassPrice = input.General.BiomassPriceAtFarmGate.GetValueOrDefault();
            _revenue.IncentivePayments = input.Financial.IncentivePayment;


        }

        public IList<CostRevenue> Flow { get; set; }
        
        public Double NPV
        {
            get
            {
                var cashFlow = new double[] {4, 5, 6, 7, 0};
                var npv = Microsoft.VisualBasic.Financial.NPV(4.5, ref cashFlow);
                Debug.Print("NPV is " + npv);
                //double dAPR = 2;
                //Int32 iNumberOfPayments = 12;
                //double dLoanAmount = 10000;
                //Console.WriteLine(Microsoft.VisualBasic.Financial.Pmt((dAPR / 100) / 12, iNumberOfPayments, dLoanAmount, 0, DueDate.EndOfPeriod) * -1);


                //Console.ReadLine();
                return npv;
            }
            set {  }
        }
    }

    public class CostRevenue
    {
        public int Year { get; set; }
        public Expenditure Expense;
        public Revenue Revenue;
    }

    public class Expenditure
    {
        public double Year { get; set;  }
        public decimal LandCost { get; set; }

        //if used, do not use admin, plantation, and maintenance cost
        public decimal ProductionCost { get; set; }

        public decimal InterestCost { get; set; }

        public decimal AdministrativeCost { get; set; }
        public decimal PlantingAndEstablishmentCost { get; set; }
        public decimal StandMaintenanceCost { get; set; }
        public decimal HarvestCost { get; set; }

        public decimal StorageCost { get; set; }
        public decimal TransportationCost { get; set; }
        public decimal TotalExpenses { get; set; }
    }

    public class Revenue
    {
        public Revenue()
        {
            IncentivePayments = 0m;
            TotalRevenue = 0m;
            BiomassPrice = 0m;
        }
        public double Year { get; set; }
        public decimal IncentivePayments { get; set; }
        public decimal BiomassPrice { get; set; }
        public decimal TotalRevenue { get; set; }
    }
}