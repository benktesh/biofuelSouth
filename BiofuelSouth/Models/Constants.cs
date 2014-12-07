using System;
using System.Collections.Generic;
using System.Linq;
using BiofuelSouth.Models;
using BiofuelSouth.Services;
using System.Data.Entity;

namespace BiofuelSouth.Models
{
    public static class Constants
    {
        
        public static List<string> GetCategory()
        {
            //TODO could change this to make database driven call.
            String[] Category = new String[] { "Select Category", "Switchgrass", "Miscanthus", "Poplar", "Willow" };
            return Category.ToList();
        }

        public static List<string> GetState()
        {
            String[] State = new String[] { "Select State", "TN", "AL", "LA", "AR", "FL", "GA", "KY", "MS", "NC", "SC", "VA" };  
            //TODO could change this to make database driven call.

            return State.ToList();
        }

        public static IEnumerable<string> GetCounty(String state)
        {
            //TODO could change this to make database driven call.
            
            var county = DataService.GetCounty(state);
            county.Insert(0,"Select County");
            return county;

        }

        public static String CountyName(String geoId)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var county = db.County.Where(c => c.GeoID == geoId).Select(a => a.Name).FirstOrDefault();
                return county;
            }

        }

        public static double GetValue()
        {
            int intGeoid = 37163;
            string category = "Switchgrass";
            using (DatabaseContext db = new DatabaseContext())
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
    }
}