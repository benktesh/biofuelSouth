using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BiofuelSouth.Models;


namespace BiofuelSouth.Services
{
    public static class DataService
    {
        public static List<string> GetCounty(String state)
        {
            using (DatabaseContext db = new DatabaseContext())
            {
                var counties = db.County.Where(p => p.State == state).Select(p=>p.Name).ToList();
                return counties;
            }  
        }


        public static double GetProductivityPerAcreForCropByGeoId(String category, String geoId)
        {
            int intGeoid = Convert.ToInt32(geoId);
            using (DatabaseContext db = new DatabaseContext())
            {
                
                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p=> p.Yield).FirstOrDefault();
                return productivity;
            }
        }

        public static double GetCostPerAcreForCropByGeoId(String category, String geoId)
        {
            int intGeoid = Convert.ToInt32(geoId);
            using (DatabaseContext db = new DatabaseContext())
            {
                var productivity = db.Productivities.Where(p => p.GeoId == intGeoid && p.CropType.Equals(category)).Select(p => p.Cost).FirstOrDefault();
                return productivity;
            }
        }
    }
}