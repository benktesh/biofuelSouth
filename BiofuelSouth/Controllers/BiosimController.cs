using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiofuelSouth.Attributes;
using BiofuelSouth.Models;
using BiofuelSouth.ViewModels;

namespace BiofuelSouth.Controllers
{
    public class BiosimController : Controller
    {
        // GET: Biosim
        [HttpGet]
        [NoCache]
        public ActionResult Index()
        {
            var model = new SimulatorViewModel();
            PopulateModel(model);
            return View(model);
        }

        [HttpPost]
        [NoCache]
        public ActionResult Index(SimulatorViewModel model)
        {
            return View(model);
        }

        void PopulateModel(SimulatorViewModel model)
        {
            model.General.CountyList = Constants.GetCountySelectList(model.General.State);
            model.General.StateList = Constants.GetState();
            model.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.General.Category);

            //            model.BiosimInput.Input.General.CountyList = 
            //            model.General.CountyList = Constants.GetCountySelectList(model.General.State);
            //            model.General.StateList = Constants.GetState();
            //            model.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.General.Category);
            //        
            //            model.CountyList = Constants.GetCountySelectList(model.State);
            //            model.StateList = Constants.GetState();
            //            model.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.Category);
        }
    }
    }
