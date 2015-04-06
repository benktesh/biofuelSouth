using System;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;
using BiofuelSouth.Models;
using log4net;
using Microsoft.Ajax.Utilities;

namespace BiofuelSouth.Controllers
{
    public class InputController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


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

        private Storage GetStorageTest()
        {
            var g = new Storage();
            g.PercentStored = 100;  
            g.RequireStorage = true; 
            g.StorageTime = 200; 
            g.StorageMethod = "1"; 
            return g;

        }

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


        //[HttpPost]
        public ActionResult Index(Input ip= null)
        {

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

            ip = null;
            Session["Input"] = ip; 

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
        }

        
        public ActionResult General(General general=null)
        {
            Input ip = (Input)Session["Input"];
            if (ip == null)
            {
                ModelState.Clear();
                ip = new Input();
            }
            
            if (general == null || general.State == null)
            {
             
                general = new General();
            
                    
                ip.General = general;
                Session["Input"] = ip; 
                return View();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View();
                }
                else
                {
                    ModelState.Clear();
                    ip = (Input)Session["Input"];
                    ip.General = general;
                    Session["Input"] = ip;
                    return RedirectToAction("Storage");
                }
            }
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



        //public ActionResult Financial(Input ip = null)
        //{
        //    if (ip == null || ip.County == null)
        //        return RedirectToAction("Index");
        //    return RedirectToAction("Index");

        //  //TODO if financial is not needed, redirect to index
        //}

        public ActionResult Storage(Storage storage=null)
        {
            Input ip = (Input)Session["Input"];
            if (ip == null)
            {
                return RedirectToAction("Index");
            }
            ip.Storage = storage;
            Session["Input"] = ip;

            //If storage is not null or storage is skipped then go to financial steps

            if (storage != null && (storage.RequireStorage != null)) //This is post back
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


            //if (ip == null || ip.General == null)
            //{
            //    return RedirectToAction("Index");
            //}

            //if (ip.Storage == null)
            //{
            //    ip.Storage = new Storage();
            //    return View();
            //}
            


            //if (ip == null || ip.County == null)
                

            //if (ip.Storage == null)
            //{
            //    ip.Storage = new Storage();
            //    ModelState.Clear();

            //}
            //else
            //{
            //    if (!ModelState.IsValid)
            //        return View(ip);

            //}
               
                
            //FillViewBag();
            //if (ModelState.IsValid)
            //{
            //    if (ip.Storage.StorageTime > 0)
            //        ip.Storage.RequireStorage = true;
            //    ViewBag.Results = true;
            //    PostSubmit(ip, "Annual Production");
            //    ViewBag.input = ip;
            //    TempData["input"] = ip;
            //    TempData.Keep();
            //}

            //return View(ip);

            //if (ip.State == null)
            //{
            //    return Index();
                
            //}

            //if (ip.Storage == null)
            //{
            //    ip.Storage = new Storage {RequireStorage = false};
                
            //    return View(ip);
            //}
                
            //if (ip.Storage.StorageTime > 0)
            //    ip.Storage.RequireStorage = true;
            
            //ViewBag.Results = true;
            //PostSubmit(ip, "Annual Production");
            //ViewBag.input = ip;
            //return View(ip);
        }

        public ActionResult Financial(Financial financial = null)
        {
            Input ip = (Input)Session["Input"];
            if (ip == null)
            {
                return RedirectToAction("Index");
            }
            ip.Financial = financial;
            Session["Input"] = ip;

            //If finance is not null or finance is skipped then go to results

            if (financial != null && (financial.RequireFinance != null)) //This is post back
            {
                if (((bool)!financial.RequireFinance) || ((bool)financial.RequireFinance && ModelState.IsValid))
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



           // cc.GenerateChart(cacheKey, rev, "Annual Production");

            

        }

    }
}