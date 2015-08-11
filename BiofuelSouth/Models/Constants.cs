using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Services;
using BiofuelSouth.ViewModels;

namespace BiofuelSouth.Models
{
    public static partial class Constants
    {

        // Weigth of bale for miscanthus was same as for the weight of switchgrass
        //http://miscanthus.illinois.edu/symposium/2009/jan_13/10_Ulrich.pdf
        private const Double WeightBaleRound = 650;
        private const Double WeightBaleRect = 2000;
        private const int BalePerStack = 6;


        private const double SqFtPerBaleRound = 36.0;
        private const double SqFtPerBaleRect = 60.0;

        private const double TarpSqFtPerStackRound = 108;
        private const double GravelSqFtPerStackRound = 108;
        private const double PalletSqFtPerStackRound = 108;

        private const double TarpSqFtPerStackRect = 180;
        private const double GravelSqFtPerStackRect = 180;
        private const double PalletSqFtPerStackRect = 180;


        public static IEnumerable<SelectListItem> GetCategory()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = @"Switchgrass", Value = "Switchgrass"},
                new SelectListItem{Text = @"Miscanthus", Value = "Miscanthus"},
                new SelectListItem{Text = @"Poplar", Value = "Poplar"},
                new SelectListItem{Text = @"Willow", Value = "Willow"}
            };
            return items;
        }





        public static IEnumerable<string> GetCounty(String state)
        {
            //TODO could change this to make database driven call.

            var county = DataService.GetCounty(state);
            county.Insert(0, "Select County");
            return county;

        }

        public static String CountyName(String geoId)
        {
            using (var db = new DatabaseContext())
            {
                var county = db.County.Where(c => c.GeoID == geoId).Select(a => a.Name).FirstOrDefault();
                return county;
            }

        }

        public static double GetValue()
        {
            const int intGeoid = 37163;
            const string category = "Switchgrass";
            using (var db = new DatabaseContext())
            {

                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p => p.Yield).FirstOrDefault();
                return productivity;
            }

        }

        //Method to return an average price of farm gate price for a crop type
        public static double GetFarmGatePrice(String cropType)
        {
            switch (cropType)
            {
                case "Switchgrass":
                    return 65.0; // http://www.uky.edu/Ag/CCD/introsheets/switchgrass.pdf
                case "Miscanthus":
                    return 45.0; //http://pubs.cas.psu.edu/FreePubs/PDFs/ee0081.pdf
                case "Poplar":
                    return 50.0;
                case "Willow":
                    return 50.0;
                default:
                    return -999.99;

            }

        }

        public static IList<double> GetProductivityTaper(String cropType)
        {
            switch (cropType)
            {
                case "Switchgrass":
                    return new List<double> { 0.25, 0.5, 1 };

                default:
                    return new List<double> { 1 };
            }
        }

        public static IEnumerable<SelectListItem> GetCountySelectList(String state = null)
        {
            IList<County> countyList = DataService.GetCountyData(state ?? "ALL");

            return countyList.Select(c => new SelectListItem { Text = c.Name, Value = c.GeoID });
        }


        public static IEnumerable<SelectListItem> GetProvincesList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = @"California", Value = "B"},
                new SelectListItem{Text = @"Alaska", Value = "B"},
                new SelectListItem{Text = @"Illinois", Value = "B"},
                new SelectListItem{Text = @"Texas", Value = "B"},
                new SelectListItem{Text = @"Washington", Value = "B"}

            };
            return items;
        }

        public static IEnumerable<SelectListItem> GetYesNo()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem {Text = @"Yes", Value = "true"},
                new SelectListItem {Text = @"No", Value = "false"},
             };
            return items;
        }

        public static double GetAvgInterestRate()
        {
            //TODO Call IRA Data to get this value here
            return 1.84;
        }

        public static IEnumerable<SelectListItem> GetState()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem {Text = @"AL", Value = "AL"},
                new SelectListItem {Text = @"AR", Value = "AR"},
                new SelectListItem {Text = @"FL", Value = "FL"},
                new SelectListItem {Text = @"GA", Value = "GA"},
                new SelectListItem {Text = @"KY", Value = "KY"},
                new SelectListItem {Text = @"MS", Value = "MS"},
                new SelectListItem {Text = @"NC", Value = "NC"},
                new SelectListItem {Text = @"SC", Value = "SC"},
                new SelectListItem {Text = @"TN", Value = "TN"},
                new SelectListItem {Text = @"TX", Value = "TX"},
                new SelectListItem {Text = @"VA", Value = "VA"}
            };
            return items;
        }

        /// <summary>
        /// Currenlty switchgrass is hardcoded.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetStorageMethod(string cropType = "Switchgrass")
        {
            
            switch (cropType)
            {
                case "Switchgrass":
                case "Miscanthus":
                    IList<SelectListItem> items = new List<SelectListItem>
                    {   //Round
                        new SelectListItem {Text = @"Round Tarp and Pallet", Value = "1"},
                        new SelectListItem {Text = @"Round Tarp and Gravel", Value = "2"},
                        new SelectListItem {Text = @"Round Tarp on Bare Ground", Value = "3"},
                        new SelectListItem {Text = @"Round Pallet No Tarp", Value = "4"},
                        new SelectListItem {Text = @"Round Gravel No Tarp", Value = "5"},
                        new SelectListItem {Text = @"Round Bare Ground No No Tarp", Value = "6"},
                        //Rectangular
                        new SelectListItem {Text = @"Rectangular Bale - Tarp and Pallet", Value = "11"},
                        new SelectListItem {Text = @"Rectangular Bale - Tarp and Gravel", Value = "12"},
                        new SelectListItem {Text = @"Rectangular Bale - No Tarp", Value = "13"},
                        new SelectListItem {Text = @"Rectangular Bale - Gravel No Tarp", Value = "14"}
                    };
                    return items;
                default:
                    return null;
            }
        }



        public static double GetBaleStorageLaborCost(double estimate, BaleType baleType, double hourlyCost, bool hasTarp = false, bool hasPallet = false)
        {

            int totalLabor = 0;

            if (hasTarp)
            {
                totalLabor++;
            }

            if (hasPallet)
            {
                totalLabor++;
            }

            if (totalLabor == 0) //If no tarp or pallet is invovled, the labor cost is 0; 
            {
                return 0;
            }


            double baleCount;
            double stack = 0;
            if (baleType == BaleType.Round)
            {
                baleCount = estimate / WeightBaleRound;
                //  stack = Math.Floor(baleCount/balePerStack);
                //  partial = baleCount%balePerStack;
            }
            else if (baleType == BaleType.Rectangular)
            {
                baleCount = estimate / WeightBaleRect;
            }
            else
            {
                return 0;

            }

            double laborCost = baleCount / BalePerStack * hourlyCost / 2 * totalLabor;     // half and hour to set one stack x total Labor cost (one for tarp and another for pallet)

            return laborCost;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="estimate">Estimated Ton Requirin Storage</param>
        /// <param name="baleType">Round or Rectangular</param>
        /// <param name="annualStorageLandCost">Annual Storage Area Land Rental Cost in $/Acre/Year. No Minimum.</param>
        /// <returns></returns>
        public static double GetBaleStorageLandCost(double estimate, BaleType baleType, double annualStorageLandCost)
        {
            double baleCount;
            // double stack = 0;
            //  double partial = 0;
            double stroageSqFt;
            if (baleType == BaleType.Round)
            {
                baleCount = estimate / WeightBaleRound;
                //stack = Math.Floor(baleCount / balePerStack);
                //partial = baleCount % balePerStack;
                stroageSqFt = GravelSqFtPerStackRound;
            }
            else if (baleType == BaleType.Rectangular)
            {
                baleCount = estimate / WeightBaleRect;
                stroageSqFt = GravelSqFtPerStackRect;
            }
            else
            {
                return 0;
            }
            double landCost = baleCount / BalePerStack * stroageSqFt * 1.10 * 0.000022956841138659 * annualStorageLandCost; //expand area of gravel by 10 % X 1/X land Cost per acre

            return landCost;
        }


        public static IList<double> GetStorageCost(Input input)
        {
            

            var cropType = (CropType)Enum.Parse(typeof(CropType), input.General.Category);
            //Return null if crop types are not miscanthus or switchgrass

            if (CropType.Switchgrass != cropType || CropType.Miscanthus != cropType)
            {
                return null;
            }
           

            StorageMethod storageMethod = 0;
            if (input.Storage.StorageMethod != null)
            {
                storageMethod =
                (StorageMethod)Enum.Parse(typeof(StorageMethod), input.Storage.StorageMethod);

            }
            
            var estimate = input.GetAnnualProductivity();
            var cftVolume = estimate / Convert.ToDouble(ConfigurationManager.AppSettings.Get("WeightToVolumeRatio"));
            var numberOfBales = -1;

            bool requiresGravel = false;
            bool requiresPallet = false;
            bool requiresTarp = false;
            BaleType baleType = BaleType.Round;


            //assumtion - 10lbs/cft
            //4 ft diamater, 5 ft height = 62 cft. ft. [pi*D2/4 *H]
            //thus one round bale weigh about 630 lbs
            //one stack cann contain 3780 lbs ir 1.89 lbs
            //one stack needs a tarp of size 15x5 sq. feet
            //75 sq. feet of tarp per 
            //cost of tarp sq. feet .15 or $11.25
            // $5.95/ton


            //every 72 months
            //2% additional cost
            //minimum 1 bale

            //if production is greater than 3780 one rate
            //if production is less than 3780 then another rate
            //for round

            //For rectangular
            //8 x 5 ft. x 5 = 200 cft
            //when stacked = 1200 cft.
            //or 12000 lbs = 6 tons
            //needs about 200 sq. ft of tarp [40 ft long and 5 ft wide for one stack] or $30
            //tarp cost per ton = $5/ton

            //Fixed cost
            //Pallet cost - about 25 cents per sq. ft. or $ 15 per stack for round and $30 per stack rect.
            //http://www.recycle.net/cgi-bin/exview.cgi?w=01&sc=1101&st=LA
            //http://ag-econ.ncsu.edu/sites/ag-econ.ncsu.edu/files/extension/budgets/forage/ForageBudHayFromStoragePrint08-848.pdf
            //Gravel cost
            //.75 cents sq foot

            //81 per 6 bales
            //27 per bale

            decimal annualStorageCost;


            double baleCount = estimate / WeightBaleRound;
            double stack = Math.Floor(baleCount / BalePerStack);
            double partial = baleCount % BalePerStack;

            double baleCountRect = estimate / WeightBaleRect;
            double stackRect = Math.Floor(baleCountRect / BalePerStack); //bale per stack is always 6.
            double partialRect = baleCountRect % BalePerStack;

            double tarpSqFtCost = (double)input.Storage.TarpCost;
            double gravelSqFtCost = (double)input.Storage.GravelCost;
            double palletSqFtCost = (double)input.Storage.PalletCost;



            //labor costs are fixed. No labor for gravel setting. Half an hour cost for tarp and pallet
            //Land costs are Annual
      
            double gravelCostRound = stack * GravelSqFtPerStackRound * gravelSqFtCost + partial * SqFtPerBaleRound * gravelSqFtCost;
            double palletCostRound = stack * PalletSqFtPerStackRound * palletSqFtCost + partial * SqFtPerBaleRound * palletSqFtCost;
            double tarpCostRound = stack * TarpSqFtPerStackRound * tarpSqFtCost + partial * SqFtPerBaleRound * tarpSqFtCost;

            double gravelCostRect = stackRect * GravelSqFtPerStackRect * gravelSqFtCost + partialRect * SqFtPerBaleRect * gravelSqFtCost;
            double palletCostRect = stackRect * PalletSqFtPerStackRect * palletSqFtCost + partialRect * SqFtPerBaleRect * palletSqFtCost;
            double tarpCostRect = stackRect * TarpSqFtPerStackRect * tarpSqFtCost + partialRect * SqFtPerBaleRect * tarpSqFtCost;


            double oneTimeCost = 0;
            IList<double> annualizedStorageCost = null;

            if (input.General != null)
            {
                annualizedStorageCost = new double[input.General.ProjectLife.GetValueOrDefault()];
            }
            else
                annualizedStorageCost = new double[10];
            //If user has supplied flat cost, then storage cost per year from user is used.
            if (input.Storage.CostOption == (int)CostEstimationOption.UserSupplyStorageCost)
            {
                annualStorageCost = input.Storage.UserEstimatedCost;

                for (int i = 0; i < annualizedStorageCost.Count(); i++)
                {
                    annualizedStorageCost[i] = (double) annualStorageCost;
                }

                return annualizedStorageCost; 
            }

            if (input.Storage.CostOption == (int)CostEstimationOption.Default || input.Storage.CostOption == (int)CostEstimationOption.UserSupplyMaterialCost)
            {

                if ((CropType.Switchgrass == cropType) || (CropType.Miscanthus == cropType))
                {
                    switch (storageMethod)
                    {
                        case StorageMethod.RoundTarpPallet:
                            {
                                oneTimeCost = palletCostRound + tarpCostRound;
                                requiresTarp = true;
                                requiresPallet = true;
                                break;
                            }

                        case StorageMethod.RoundTarpGravel:
                            oneTimeCost = gravelCostRound + tarpCostRound;
                            requiresTarp = true;
                            requiresPallet = true;
                            requiresGravel = false;
                            break;
                        case StorageMethod.RoundTarpBareGroud:
                            oneTimeCost = tarpCostRound;
                            requiresTarp = true;
                            requiresPallet = false;
                            requiresGravel = false;
                            break;
                        case StorageMethod.RoundPalletNoTarp:
                            oneTimeCost = palletCostRound;
                            requiresTarp = false;
                            requiresPallet = true;
                            requiresGravel = false;
                            break;
                        case StorageMethod.RoundGravelNoTarp:
                            oneTimeCost = gravelCostRound;
                            requiresTarp = false;
                            requiresPallet = false;
                            requiresGravel = true;
                            break;
                        case StorageMethod.RoundBareGroundNoTarp:
                            oneTimeCost = gravelCostRound;
                            requiresTarp = false;
                            requiresPallet = false;
                            requiresGravel = false;

                            break;
                        case StorageMethod.RectangularTarpPallet:
                            oneTimeCost = palletCostRect + tarpCostRect;
                            requiresTarp = true;
                            requiresPallet = true;
                            requiresGravel = false;
                            baleType = BaleType.Rectangular;
                            break;
                        case StorageMethod.RectangularNoTarp:
                            oneTimeCost = 0;
                            requiresTarp = false;
                            requiresPallet = false;
                            requiresGravel = false;
                            baleType = BaleType.Rectangular;
                            break;
                        case StorageMethod.RectangularGravelNoTarp:
                            oneTimeCost = gravelCostRect;
                            requiresTarp = false;
                            requiresPallet = false;
                            requiresGravel = true;
                            baleType = BaleType.Rectangular;
                            break;
                    }
                    annualizedStorageCost = GetAnnualStorageCost(oneTimeCost, input.General.ProjectLife.GetValueOrDefault());

                    //Add land and labor cost
                    var annualProduction = input.GetAnnualProductionList();

                    for (int i = 0; i < annualizedStorageCost.Count(); i++)
                    {
                        double laborCost = GetBaleStorageLaborCost(annualProduction[i] * input.Storage.PercentStored / 100, baleType, input.Storage.LaborCost.GetValueOrDefault(), requiresTarp, requiresPallet);
                        double landCost = GetBaleStorageLandCost(annualProduction[i] * input.Storage.PercentStored / 100, baleType, input.Storage.LandCost.GetValueOrDefault());

                        annualizedStorageCost[i] = annualizedStorageCost[i] + laborCost + landCost;
                    }
                    return annualizedStorageCost;

                }
            }

            return annualizedStorageCost; 
        }

        private static IList<double> GetAnnualStorageCost(double oneTimeCost, int projectLife = 10)
        {

            IList<double> annualizedStorageCost = new double[projectLife];
            //First year one time cost
            //year 2 - 5, put two percent of the annual cost
            //every 6th years, annualCost + 8% and then continue to two percent
            var tempOneTimeCost = oneTimeCost;
            const double percentIncrement = 0.02;
            for (int i = 0; i < projectLife; i++)
            {
                if (i == 0)
                {
                    annualizedStorageCost[i] = oneTimeCost;
                }
                else if (i > 0 && i % 5 == 0)
                {
                    tempOneTimeCost = tempOneTimeCost * (1+ percentIncrement * i);
                    annualizedStorageCost[i] = tempOneTimeCost;

                }
                else if (i > 0 && i % 5 != 0)
                {
                    annualizedStorageCost[i] = tempOneTimeCost * percentIncrement;
                }

            }

            return annualizedStorageCost;

        }

        
        public static Double GetStorageLoss(int storageMethod, string cropType = null)
        {
            var result = 0.0;
            if (cropType == null)
            {
                cropType = "SwitchGrass";
            }

            if (cropType.ToLower().Equals("switchgrass") || cropType.ToLower().Equals("miscanthus"))
            {

                switch (storageMethod)
                {
                    case 1:
                        result = 1.0;
                        break;
                    case 2:
                        result = 8.5;
                        break;
                    case 3:
                        result = 7.0;
                        break;
                    case 4:
                        result = 18.2;
                        break;
                    case 5:
                        result = 16.6;
                        break;
                    case 6:
                        result = 12.8;
                        break;
                    case 11:
                        result = 13.7;
                        break;
                    case 12:
                        result = 28.0;
                        break;
                    case 13:
                        result = 48.0;
                        break;
                    case 14:
                        result = 57.1;
                        break;
                    default:
                        result = 0;
                        break;
                }

            }

            return result;
        }

    }
}