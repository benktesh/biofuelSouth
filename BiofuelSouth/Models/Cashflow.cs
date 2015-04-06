using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Http.ValueProviders.Providers;


namespace BiofuelSouth.Models
{
    public class Cashflow
    {
        

        public Expenditure _expenditure { get; set; }
        Revenue _revenue { get; set; }
        int Duration { get; set; }
        public Cashflow(Input input)
        {
            
            Duration = input.General.ProjectLife;

            _expenditure.AdministrativeCost = input.Financial.AdministrativeCost;
            _expenditure.LandCost = input.General.LandCost;
            _expenditure.PlantingAndEstablishmentCost = 0;  
            _expenditure.StandMaintenanceCost = 0;
            _expenditure.HarvestCost = 0;
            _expenditure.StorageCost = 0; 
            _expenditure.TransportationCost = 0;

            _revenue.BiomassPrice = input.General.BiomassPriceAtFarmGate;
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
        public double LandCost { get; set; }

        //if used, do not use admin, plantation, and maintenance cost
        public double ProductionCost { get; set; }

        public double AdministrativeCost { get; set; }
        public double PlantingAndEstablishmentCost { get; set; }
        public double StandMaintenanceCost { get; set; }
        public double HarvestCost { get; set; }

        public double StorageCost { get; set; }
        public double TransportationCost { get; set; }
        public double TotalExpenses { get; set; }
    }

    public class Revenue
    {
        public double Year { get; set; }
        public double IncentivePayments { get; set; }
        public double BiomassPrice { get; set; }
        public double TotalRevenue { get; set; }
    }
}