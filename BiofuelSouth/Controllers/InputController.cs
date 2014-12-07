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
        // GET: Input
        [HttpGet]
        public ActionResult Index()
        {
            //Constants.GetValue();
            FillViewBag();
          
            // ViewBag.County = constants.GetCounty();
            Input input = new Input();
            if (ViewBag.input != null)
                input = ViewBag.input;
            
            
            return View(input);
        }

        [HttpPost]
        public ActionResult Index(Input input)
        {
            FillViewBag();
            ViewBag.Results = true;
            //TODO Do some thing here
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

        }

    }
}