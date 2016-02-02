using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Enum;
using BiofuelSouth.Manager;
using BiofuelSouth.Models;
using BiofuelSouth.Services;
using BiofuelSouth.ViewModels;

namespace BiofuelSouth.Controllers
{
    public class DSSController : Controller
    {
        //private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /*
                private General GetGeneralTest()
                {
                    var g = new General();
                    g.Category = "Switchgrass";
                    g.State = "AL";
                    g.County = "01001";
                    g.ProjectSize = 100;
                    g.ProjectLife = 10;
                    g.BiomassPriceAtFarmGate = 100;
                    g.LandCost = 20;
                    return g;

                }
        */

        /*
                private Storage GetStorageTest()
                {
                    var g = new Storage();
                    g.PercentStored = 100;  
                    g.RequireStorage = true; 
                    g.StorageTime = 200; 
                    g.StorageMethod = "1"; 
                    return g;

                }
        */

        /*
                private Financial GetFinancialTest()
                {
                    var f = new Financial();
                    f.RequireFinance = true;
                    f.LoanAmount = 1000;
                    f.InterestRate = Constants.GetAvgInterestRate();
                    f.EquityLoanInterestRate = 8;
                    f.AdministrativeCost = 100;
                    f.AvailableEquity = 1000;
                    f.LoanAmount = 10000;
                    f.IncentivePayment = 0; 
                    f.YearsOfIncentivePayment = 0; 

                    return f; 

                }
        */

        [HttpGet]
        public ActionResult Index()
        {
            Input ip = new Input();
            Session["Input"] = ip;
            PopulateHelpers(ip);
            return RedirectToAction("General");
        }

        [HttpPost]
        public ActionResult Index(Input ip)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            if (ip == null)
            {
                ip = new Input();
            }
            Session["Input"] = ip;

            PopulateHelpers(ip);

            ModelState.Clear();
            return RedirectToAction("General");
        }


        [HttpGet]
        public ActionResult General()
        {
            var ip = InputGet();
            PopulateHelpers(ip);
            Session["Input"] = ip;
            return View(ip.General);
        }

        [HttpPost]
        public ActionResult General(General general)
        {

            if (!ModelState.IsValid)
            {
                PopulateHelpers(general);
                return View(general);
            }

            ModelState.Clear();
            var ip = InputGet();
            if (ip == null)
            {
                return RedirectToAction("General");
            }
            
            ip.General = general;
            Session["Input"] = ip;
            return RedirectToAction("GetProductionCost");

        }


        private Input InputGet()
        {
            var ip = Session["Input"] ?? new Input();
	        return (Input) ip;  
        }

        private Input InputSet(Input input = null)
        {
	        if (input == null)
	        {
				Session.Remove( "Input" );
			}
            Session["Input"] = input;
            return InputGet(); 
        }

        [HttpGet]
        public ActionResult Storage()
        {
            var ip = (Input)Session["Input"];
            if (ip == null)
            {
                return RedirectToAction("Index");
            }
            var storage = ip.Storage;
            if (storage == null)
            {
                storage = new Storage();
            }

            storage.CurrentStep = WizardStep.Storage;
            storage.PreviousAction = "GetProductionCost";

           

            

            if (ip.General.Category == CropType.Poplar || ip.General.Category == CropType.Pine || ip.General.Category == CropType.Willow)
            {
                storage.RequireStorage = false;
                ip.Storage = storage;
                Session["Input"] = ip;
                return RedirectToAction("Financial");
            }

            ip.Storage = storage;
            Session["Input"] = ip;
            return View(storage);


            

        }

        [HttpPost]
        public ActionResult Storage(Storage storage)
        {
            
            var ip = (Input)Session["Input"];
            if (ip == null || storage == null) //if session input is null or storage is null, return to index
            {
                return RedirectToAction("Index");
            }

            if (!storage.RequireStorage.GetValueOrDefault()) //if storage is indicated as not required, clear the model state;  
            {
                ModelState.Clear();
            }
            //if (storage == null)
            //{
            //    storage = new Storage();
            //    storage.CurrentStep = WizardStep.Storage;
            //    storage.PreviousAction = "GetProductionCost";
            //}

            if (!ModelState.IsValid)
            {
                return View("Storage", storage);
            }


            if (ModelState.IsValid && ip.General.Category == CropType.Poplar || ip.General.Category == CropType.Pine || ip.General.Category == CropType.Willow)
            {
                storage.RequireStorage = false;
            }


            ip.Storage = storage;
            Session["Input"] = ip;

            return RedirectToAction("Financial");
            
        }

        [HttpGet]
        public ActionResult GetProductionCost()
        {
            var model = GetProductionCostViewModel(); 
            return View("_productionCost", model);
        }

        public ActionResult NewDSS()
        {
            Session["Input"] = null;
            Session.Remove("Input");
            return RedirectToAction("General");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProductionCost(ProductionCostViewModel model)
        {

            if (InputGet() == null)
            {
                return RedirectToAction("General");
            }

            if (ModelState.IsValid)
            {

                model.UseCustom = true;
                if (model.CropType == CropType.Miscanthus || model.CropType == CropType.Switchgrass)
                {
                    model.UseCustom = false;
                }
                    var input = (Input)Session["Input"];
                input.ProductionCost = model;
                Session["Input"] = input;

                if (input.General.Category == CropType.Miscanthus || input.General.Category == CropType.Switchgrass)
                    return RedirectToAction("Storage");
                return RedirectToAction("Financial");
            }

            return View("_productionCost", model);
        }

        private ProductionCostViewModel GetProductionCostViewModel()
        {
            ProductionCostManager pcm = new ProductionCostManager();

            var input = (Input)Session["Input"];
            return pcm.GetProductionCost(new ProductionCostViewModel { CropType = input.General.Category, County = input.General.County, UseCustom = true});
        }

        [HttpGet]
        public ActionResult Financial()
        {
            var ip = InputGet();
            if (ip == null)
            {
                return RedirectToAction("Index");
            }

            var financial = ip.Financial ?? new Financial();

            financial.CurrentStep = WizardStep.Financial;
            
            if (ip.General.Category == CropType.Miscanthus || ip.General.Category == CropType.Switchgrass)
            {

                financial.PreviousAction = "Storage";
            }
            else
            {
                financial.PreviousAction = "GetProductionCost";
            }

            ip.Financial = financial;
            Session["Input"] = ip;
           
            ModelState.Clear();
            return View(financial);

        }

        [HttpPost]
        public ActionResult Financial(Financial financial)
        {
            var ip = (Input)Session["Input"];
            if (ip == null || financial == null)
            {
                return RedirectToAction("Index");
            }


            if (financial.RequireFinance.HasValue && !financial.RequireFinance.Value)
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                financial.CurrentStep = WizardStep.Result;
                ip.Financial = financial;
                Session["Input"] = ip;
                return RedirectToAction("Results");
            }

            return View("Financial", financial);

            //If finance is not null or finance is skipped then go to results

            if (financial != null && (financial.RequireFinance != null)) //This is post back
            {
                if (!financial.RequireFinance.GetValueOrDefault())
                {

                    financial = new Financial();
                    ip.Financial = financial;
                }

                if ((!financial.RequireFinance.GetValueOrDefault()) || (financial.RequireFinance.GetValueOrDefault() && ModelState.IsValid))
                {
                    financial.CurrentStep = WizardStep.Result;
                    return RedirectToAction("Results");
                }
                financial.CurrentStep = WizardStep.Financial;
                return View(financial);
            }



            ModelState.Clear();
            return View(financial);

        }

        [HttpGet]
        public ActionResult GetAlternative(CropType cropType)
        {

            //SwapCrop
            //GEnerate results

            return null;  

        }


        private CropType? GetPrimaryCrop()
        {
	        InputSet(null);
            var input = InputGet();

            return input?.General.Category;
        }

       
        public ActionResult Results()
        {
            var input = InputGet();
            if (input == null)
            {
                return RedirectToAction("General");
            }

            var vm = new Simulator(input);

            var vms = vm.GetViewModels();
            return View("TabbedResult", vms[0]);
        }

        private Input GetDefaultInput()
        {
            var ip = new Input();
            ip.General.Category = CropType.Willow;
            ip.General.ProjectLife = 10;
            ip.General.ProjectSize = 100;
            ip.General.State = "AL";
            ip.General.County = "01001";
            ip.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(ip.General.Category);
            ip.General.LandCost = 70;
            ip.ProductionCost = GetProductionCostViewModel();
            
            return ip;
        }

		[HttpGet]
        public ActionResult TestTabbedResult()
        {
            var input = GetDefaultInput();
            
            var vm = new Simulator(input);
           
            var vms = vm.GetViewModels();
            Session["ViewModels"] = vms;

            return View("TabbedResult",vms[0]);

        }

        private void PostSubmit(Input ip)
        {
            var c = new ChartController();
            var revenueCachekey = Guid.NewGuid().ToString();
            ViewBag.RevenueCachekey = revenueCachekey;
            //c.GenerateCostRevenueChart(costRevenueCachekey, ip.GetRevenues().Select(m => m.TotalRevenue).ToArray(), "Cost and Revenue");
            var rev = ip.GetRevenues().Select(m => m.TotalRevenue).ToArray();
            c.GenerateChart(revenueCachekey, rev, "Revenue");

            var AnnualProd = ip.GetAnnualProductionList().ToArray();
            var cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey = cacheKey;
            var cc = new ChartController();
            cc.GenerateChart(cacheKey, AnnualProd.Select(m=> (decimal)m).ToArray(), "Annual Production");

            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey1 = cacheKey;
            cc.GenerateCostRevenueChart(cacheKey, ip, "Cost and Revenue");

            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey3 = cacheKey;
            cc.GenerateColumnChart(cacheKey, ip.GetCashFlow().Select(m=>(decimal)m).ToArray(), "Cash Flow", "Year ", "$");

        }

        private void PopulateHelpers(Input model)
        {

            model.General.CountyList = Constants.GetCountySelectList(model.General.State);
            model.General.StateList = Constants.GetState();
            model.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.General.Category);

        }

        private void PopulateHelpers(General model)
        {
            model.CountyList = Constants.GetCountySelectList(model.State);
            model.StateList = Constants.GetState();
            model.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.Category);
        }


    }


}