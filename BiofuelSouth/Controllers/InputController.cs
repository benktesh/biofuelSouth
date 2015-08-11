using System;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Models;

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
        public ActionResult Index(Input ip= null)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            //TODO - Testing financials -
            /*
            ip = (Input)Session["Input"];
            ip = new Input();
            General general = GetGeneralTest();
            Storage storage = GetStorageTest();
            Financial financial = GetFinancialTest();
            

            ip.General = general;
            ip.Storage = storage;
            ip.Financial = financial;
    
            Session["Input"] = ip;

            return RedirectToAction("Financial", financial);

            */

            if (ip == null)
            {
                ip = new Input();
            }
            Session["Input"] = ip; 

            PopulateHelpers(ip);

            ModelState.Clear();
            return RedirectToAction("General");
            //create a session variable

            //ModelState.Clear();

            //if (ip == null || ip.ProjectLife == 0)
            //{
            //    FillViewBag();
            //    //ip = new Input();
            //    //ip.General = new General();
            //    //Session["Input"] = ip;

            //    //return View(ip);
            //    return RedirectToAction("General");
            //}
            //else
            //{
            //    Session["Input"] = ip;
            //}
                
            //TODO Do some thing here
/*
            if (ip.Storage != null && ip.Storage.StorageTime > 0)
            {
                ip.Storage.RequireStorage = true;
            }
            
     
                FillViewBag();
            
            if (ModelState.IsValid)
            {
                PostSubmit(ip);
                TempData["input"] = ip;
                TempData.Keep();
                ViewBag.Results = true;
                
            }
            return View(ip);
*/
        }

        
        public ActionResult General(General general=null)
        {
            var ip = (Input)Session["Input"];
            if (ip == null)
            {
                ModelState.Clear();
                ip = new Input();
                ip.General = new General();
                PopulateHelpers(ip);
                Session["Input"] = ip; 
                return View(ip.General);
            }

            if (!ModelState.IsValid)
            {
                ip.General = general;
                PopulateHelpers(ip);
                Session["Input"] = ip; 
                return View(ip.General);
            }
           
            //if (general == null || general.State == null)
            //{

            //    general = new General();
            //    ip.General = general;
            //    Session["Input"] = ip;
            //    PopulateHelpers(ip);
            //    return View(ip.General);
            //}

           
            ModelState.Clear();
            ip = (Input)Session["Input"];
            ip.General = general;
            Session["Input"] = ip;
            return RedirectToAction("Storage");
        }

      
        

/*
        private void FillViewBag()
        {
            
            var category = Constants.GetCategory();
            ViewBag.Category = category;
            var state = Constants.GetState();
            ViewBag.State = state;
            ViewBag.Results = false;
            ViewBag.StorageMethod = Constants.GetStorageMethod();

        }
*/



     public ActionResult Storage(Storage storage=null)
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

         if (ip.General.Category == CropType.Poplar.ToString())
         {
             storage.RequireStorage = false;
         }

            if (storage.RequireStorage != null) //This is post back
            {
               
                
                if ((bool) !storage.RequireStorage)
                {
                    return RedirectToAction("Financial");
                }

                if ((bool) storage.RequireStorage && ModelState.IsValid)
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
                    TempData["input"] = ip;
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

            var cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey = cacheKey;
            var cc = new ChartController();
            cc.GenerateChart(cacheKey, ip.GetAnnualProductionList().ToArray(), "Annual Production");
            cacheKey = Guid.NewGuid().ToString();
            ViewBag.cacheKey1 = cacheKey;
            cc.GenerateCostRevenueChart(cacheKey, ip, "Cost and Revenue");

        }

        private void PopulateHelpers(Input model)
        {
            
            model.General.CountyList = Constants.GetCountySelectList(model.General.State);
            model.General.StateList = Constants.GetState(); 

        }

    }


}