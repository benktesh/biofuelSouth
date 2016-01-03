using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using BiofuelSouth.Models;
using log4net;

namespace BiofuelSouth.Controllers
{
    public class DataController :Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

      

        public ActionResult  CountiesForState(string state = "ALL")
        {
            
            try
            {
                using (var db = new DatabaseContext())
                {

                    IQueryable<County> counties = db.County;

                    if (state != "ALL")
                    {
                        counties = db.County.Where(p => p.State == state);
                    }
                    


                    var result = (from s in counties
                                  select new
                                  {
                                      id = s.CountyCode,
                                      name = s.Name
                                  }).ToList();
                    return Json(result, JsonRequestBehavior.AllowGet);
                    
                }
               // DatabaseContext db = new DatabaseContext();
              
            }
            catch (Exception exception)
            {
                Log.Error("Error processing counties" + exception);
            }
            return null;


        }
    }
}
