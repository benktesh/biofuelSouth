using System.Web.Mvc;

namespace BiofuelSouth.Controllers
{
    public class MapController : Controller
    {
        // GET: Map
        //ID 1 = Switchgrass
        //ID 2 = Miscanthus
        //ID 3 = Energy Cane
        //ID 4 = Poplar
        //ID 5 = Willow

        public ActionResult Index()
        {
            return View();
        }

        [Route("map/{1}/productivity")]
        public ActionResult productivity(int id)
        {
            return View();
        }
    }
}