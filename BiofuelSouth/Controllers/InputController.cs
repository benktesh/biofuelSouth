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
            Constants constants = new Constants();

            var category = constants.GetCategory();
            category.Insert(0, "Select Category");
            ViewBag.Category = category;

            var state = constants.GetState();
            state.Insert(0,"Select State");
            ViewBag.State = state;
            
            
            // ViewBag.County = constants.GetCounty();
            Input input = new Input();
            
            return View(input);
        }

        [HttpPost]
        public ActionResult Index(Input input)
        {
            //TODO Do some thing here
            return View(input);
        }

    }
}