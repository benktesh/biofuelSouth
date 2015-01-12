using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Models;

namespace BiofuelSouth.Controllers
{
    public class DataController :Controller
    {
        public ActionResult  CountiesForState(string state)
        {
            DatabaseContext db = new DatabaseContext();
            var counties = db.County.Where(p => p.State == state);
            var result = (from s in counties
                select new
                {
                    id = s.CountyCode,
                    name = s.Name
                }).ToList();
            return Json(result, JsonRequestBehavior.AllowGet);
        }
    }
}
