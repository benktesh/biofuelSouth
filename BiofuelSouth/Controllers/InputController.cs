using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiofuelSouth.Models;

namespace BiofuelSouth.Controllers
{
    public class InputController : Controller
    {
        private Input input = new Input();
        // GET: Input
        [HttpGet]
        public ActionResult Index()
        {
            //Constants.GetValue();
            FillViewBag();
          
            // ViewBag.County = constants.GetCounty();
            
            if (ViewBag.input != null)
                input = ViewBag.input;
            else
            {
                ViewBag.input = input; 
            }
            
            
            return View(input);
        }

        [HttpPost]
        public ActionResult Index(Input ip)
        {
            input = ip; 
            
            FillViewBag();
            ViewBag.Results = true;
            //TODO Do some thing here
            if (input.StorageRequirement != null && input.StorageRequirement.StorageTime > 0)
            {
                input.StorageRequirement.RequireStorage = true;
            }
            PostSubmit(ip, "Annual Production");
            ViewBag.input = input;
            return View(input);
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
            if (ViewBag.input == null)
                return Index();
            input = ViewBag.input; 
          return  Storage(input);
        }

        [HttpPost]
        public ActionResult Storage(Input ip)
        {
            input = ip; 
            if (input.StorageRequirement.StorageTime > 0)
                input.StorageRequirement.RequireStorage = true;
            FillViewBag();
            ViewBag.Results = true;
            PostSubmit(ip, "Annual Production");
            return View(input);
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