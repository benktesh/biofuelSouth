using System;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Models;
using Microsoft.Ajax.Utilities;

namespace BiofuelSouth.Controllers
{
    public class InputController : Controller
    {
        //private Input input = new Input();
        // GET: Input
        [HttpGet]
        public ActionResult Index()
        {
            var input = new Input();
            FillViewBag();
            if (TempData["input"] == null)
            {   
                TempData["input"] = input;
              
            }
            else
            {
                input = TempData["input"] as Input;


            }
            TempData.Keep();
            //Constants.GetValue();
            
          
            // ViewBag.County = constants.GetCounty();
            
            //if (ViewBag.input != null)
            //    input = ViewBag.input;
            //else
            //{
            //    ViewBag.input = input; 
            //}
            
            
            return View(input);
        }

        [HttpPost]
        public ActionResult Index(Input ip)
        {
            
            
            
            
           
            //TODO Do some thing here
            if (ip.StorageRequirement != null && ip.StorageRequirement.StorageTime > 0)
            {
                ip.StorageRequirement.RequireStorage = true;
            }
            
            
                FillViewBag();
            
            if (ModelState.IsValid)
            {
                PostSubmit(ip, "Annual Production");
                TempData["input"] = ip;
                TempData.Keep();
                ViewBag.Results = true;
                
            }
            return View(ip);
        }

        private void FillViewBag()
        {
            
            var category = Constants.GetCategory();
            ViewBag.Category = category;
            var state = Constants.GetState();
            ViewBag.State = state;
            ViewBag.Results = false;
            ViewBag.StorageMethod = Constants.GetStorageMethod();

        }

        [HttpGet]
        public ActionResult Storage()
        {
            FillViewBag();
            var ip = TempData["input"] as Input;
            if (ip != null && ip.State != null)
            {
                TempData.Keep();
                return View(ip);
            }
            return RedirectToAction("Input");
            
        }

        [HttpPost]
        public ActionResult Storage(Input ip)
        {
            FillViewBag();
            if (ModelState.IsValid)
            {
                if (ip.StorageRequirement.StorageTime > 0)
                    ip.StorageRequirement.RequireStorage = true;
                ViewBag.Results = true;
                PostSubmit(ip, "Annual Production");
                ViewBag.input = ip;
                TempData["input"] = ip;
                TempData.Keep();
            }

            return View(ip);

            if (ip.State == null)
            {
                return Index();
                
            }

            if (ip.StorageRequirement == null)
            {
                ip.StorageRequirement = new Storage {RequireStorage = false};
                
                return View(ip);
            }
                
            if (ip.StorageRequirement.StorageTime > 0)
                ip.StorageRequirement.RequireStorage = true;
            
            ViewBag.Results = true;
            PostSubmit(ip, "Annual Production");
            ViewBag.input = ip;
            return View(ip);
        }

        private void PostSubmit(Input ip, String chartName)
        {
            string cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey = cacheKey;
            var cc = new ChartController();
            cc.GenerateChart(cacheKey, ip.GetAnnualProductionList().ToArray(), chartName);

        }

    }
}