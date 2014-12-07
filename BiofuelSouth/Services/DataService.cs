using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using BiofuelSouth.Controllers;
using DbContext = BiofuelSouth.Models.DataBaseContext;


namespace BiofuelSouth.Services
{
    public static class DataService
    {
        public static List<string> GetCounty(String state)
        {
            using (DbContext db = new DbContext())
            {
                var counties = db.County.Where(p => p.State == state).Select(p=>p.Name).ToList();
                return counties;
            }  
        }


        public static double GetProductivityPerAcreForCropByGeoId(String category, String geoId)
        {
            int intGeoid = Convert.ToInt32(geoId);
            using (DbContext db = new DbContext())
            {
                var productivity = db.Productivity.Where(p => p.CountyId == intGeoid && p.CropType.Equals(category)).Select(p=> p.Yield).FirstOrDefault();
                return productivity;
            }
        }

        public static double GetCostPerAcreForCropByGeoId(String category, String geoId)
        {
            int intGeoid = Convert.ToInt32(geoId);
            using (DbContext db = new DbContext())
            {
                var productivity = db.Productivity.Where(p => p.CountyId == intGeoid && p.CropType.Equals(category)).Select(p => p.Cost).FirstOrDefault();
                return productivity;
            }
        }
    }
}