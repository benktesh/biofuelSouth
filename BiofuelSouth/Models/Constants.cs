using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Web.Mvc;
using BiofuelSouth.Services;

namespace BiofuelSouth.Models
{
    public static partial class Constants
    {

        public static IEnumerable<SelectListItem> GetCategory()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "Switchgrass", Value = "Switchgrass"},
                new SelectListItem{Text = "Miscanthus", Value = "Miscanthus"},
                new SelectListItem{Text = "Poplar", Value = "Poplar"},
                new SelectListItem{Text = "Willow", Value = "Willow"}
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
            IList<SelectListItem> items = null;
            IList<County> countyList = DataService.GetCountyData(state ?? "ALL");

            return countyList.Select(c => new SelectListItem() { Text = c.Name, Value = c.GeoID});
        }
              

        public static IEnumerable<SelectListItem> GetProvincesList()
        {
            IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem{Text = "California", Value = "B"},
                new SelectListItem{Text = "Alaska", Value = "B"},
                new SelectListItem{Text = "Illinois", Value = "B"},
                new SelectListItem{Text = "Texas", Value = "B"},
                new SelectListItem{Text = "Washington", Value = "B"}

            };
            return items;
        }

        public static IEnumerable<SelectListItem> GetYesNo()
        {
              IList<SelectListItem> items = new List<SelectListItem>
            {
                new SelectListItem {Text = "Yes", Value = "true"},
                new SelectListItem {Text = "No", Value = "false"},
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
                new SelectListItem {Text = "AL", Value = "AL"},
                new SelectListItem {Text = "AR", Value = "AR"},
                new SelectListItem {Text = "FL", Value = "FL"},
                new SelectListItem {Text = "GA", Value = "GA"},
                new SelectListItem {Text = "KY", Value = "KY"},
                new SelectListItem {Text = "MS", Value = "MS"},
                new SelectListItem {Text = "NC", Value = "NC"},
                new SelectListItem {Text = "SC", Value = "SC"},
                new SelectListItem {Text = "TN", Value = "TN"},
                new SelectListItem {Text = "TX", Value = "TX"},
                new SelectListItem {Text = "VA", Value = "VA"}
            };
            return items;
        }

        /// <summary>
        /// Currenlty switchgrass is hardcoded.
        /// </summary>
        /// <returns></returns>
        public static IEnumerable<SelectListItem> GetStorageMethod()
        {
            const string cropType = "Switchgrass";
            switch (cropType)
            {
                case "Switchgrass":
                    IList<SelectListItem> items = new List<SelectListItem>
                    {   //Round
                        new SelectListItem {Text = "Round Tarp and Pallet", Value = "1"},
                        new SelectListItem {Text = "Round Tarp and Gravel", Value = "2"},
                        new SelectListItem {Text = "Round Tarp on Bare Ground", Value = "3"},
                        new SelectListItem {Text = "Round Pallet No Tarp", Value = "4"},
                        new SelectListItem {Text = "Round Gravel No Tarp", Value = "5"},
                        new SelectListItem {Text = "Round Bare Ground No No Tarp", Value = "6"},
                        //Rectangular
                        new SelectListItem {Text = "Rectangular Bale - Tarp and Pallet", Value = "11"},
                        new SelectListItem {Text = "Rectangular Bale - Tarp and Gravel", Value = "12"},
                        new SelectListItem {Text = "Rectangular Bale - No Tarp", Value = "13"},
                        new SelectListItem {Text = "Rectangular Bale - Gravel No Tarp", Value = "14"}
                    };
                    return items;
                default:
                    return null;
            }
        }

        public static Double GetStorageLoss(int storageMethod, string cropType)
        {
            var result = 0.0;

            cropType = "SwitchGrass";
            if (cropType.ToLower().Equals("switchgrass"))
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