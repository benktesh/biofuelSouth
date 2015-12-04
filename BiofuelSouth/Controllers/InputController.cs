using System;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Enum;
using BiofuelSouth.Manager;
using BiofuelSouth.Models;
using BiofuelSouth.ViewModels;

namespace BiofuelSouth.Controllers
{
    public class InputController : Controller
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
            var ip = new Input();
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
            var ip = (Input)Session["Input"];
            ip.General = general;
            Session["Input"] = ip;
            return RedirectToAction("GetProductionCost");

        }


        public ActionResult Storage(Storage storage = null)
        {
            var ip = (Input)Session["Input"];
            if (ip == null)
            {
                return RedirectToAction("Index");
            }
            if (storage == null)
                storage = new Storage();

            ip.Storage = storage;
            Session["Input"] = ip;

            //If storage is not null or storage is skipped then go to financial steps

            if (ip.General.Category == CropType.Poplar)
            {
                storage.RequireStorage = false;
            }

            if (storage.RequireStorage != null) //This is post back
            {


                if ((bool)!storage.RequireStorage)
                {
                    return RedirectToAction("Financial");
                }

                if ((bool)storage.RequireStorage && ModelState.IsValid)
                {
                    return RedirectToAction("Financial");
                }


                return View(storage);


            }


            storage = new Storage();


            if (ip.Storage == null)
            {
                ip.Storage = storage;
            }

            ModelState.Clear();
            return View(storage);
        }

        [HttpGet]
        public ActionResult GetProductionCost()
        {
            var model = GetProductionCostViewModel(); 
            return View("_productionCost", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProductionCost(ProductionCostViewModel model)
        {
            if (ModelState.IsValid)
            {
                var input = (Input)Session["Input"];
                input.ProductionCost = model;
                Session["Input"] = input;

                if (input.General.Category == CropType.Miscanthus || input.General.Category == CropType.Switchgrass)
                    return RedirectToAction("Storage");
                return RedirectToAction("Financial");
            }
            PopulateHelpers(model);
            return View("_productionCost", model);
        }

        private ProductionCostViewModel GetProductionCostViewModel()
        {
            ProductionCostManager pcm = new ProductionCostManager();

            var input = (Input)Session["Input"];
            return pcm.GetProductionCost(new ProductionCostViewModel { CropType = input.General.Category, County = input.General.County });
        }
        public ActionResult Financial(Financial financial = null)
        {
            var ip = (Input)Session["Input"];
            if (ip == null)
            {
                return RedirectToAction("Index");
            }
            ip.Financial = financial;
            Session["Input"] = ip;

            //If finance is not null or finance is skipped then go to results

            if (financial != null && (financial.RequireFinance != null)) //This is post back
            {
                if (!financial.RequireFinance.GetValueOrDefault())
                {
                    ip.Financial = new Financial();
                    financial = new Financial();
                }

                if ((!financial.RequireFinance.GetValueOrDefault()) || (financial.RequireFinance.GetValueOrDefault() && ModelState.IsValid))
                {

                    PostSubmit(ip);
                    TempData["Input"] = ip;
                    TempData.Keep();
                    ViewBag.Results = true;
                    return View("Result", ip);
                }

                return View(financial);
            }


            if (financial == null)
            {
                financial = new Financial();
            }

            if (ip.Financial == null)
            {
                ip.Financial = financial;
            }

            ModelState.Clear();
            return View(financial);

        }

        public ActionResult Result(Input ip)
        {
            return View(ip);
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
            cc.GenerateChart(cacheKey, AnnualProd, "Annual Production");

            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey1 = cacheKey;
            cc.GenerateCostRevenueChart(cacheKey, ip, "Cost and Revenue");

            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey3 = cacheKey;
            cc.GenerateColumnChart(cacheKey, ip.GetCashFlow(), "Cash Flow", "Year ", "$");

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

        private void PopulateHelpers(ProductionCostViewModel model)
        {

        }


        #region newLogic

        [HttpGet]
        public ActionResult GetStorage()
        {
            WizardViewModel cachedModel = (WizardViewModel)Session["WizardViewModel"];
            if (cachedModel == null)
            {
                return Redirect("Index");
            }

            return View("_productionCost", cachedModel.ProductionCostView);
        }

        [HttpPost]
        public ActionResult GetStorage(ProductionCostViewModel vm)
        {
            if (ModelState.IsValid)
            {

            }
            return View("_productionCost", vm);

        }

        #endregion

    }


}