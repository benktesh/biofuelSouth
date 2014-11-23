using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using BiofuelSouth.Controllers;


namespace BiofuelSouth.Services
{
    public static class DataService
    {

        public static List<string> GetCounty(String state)
        {
            DatabaseContext db = new DatabaseContext();
            var counties = db.County.Where(p=>p.State == state);
            var c = counties.Select(p=>p.Name).ToList();

            return c;
        }
    }
}