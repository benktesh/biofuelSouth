﻿using System;
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
        //private Input input = new Input();
        // GET: Input
        [HttpGet]
        public ActionResult Index1()
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
          
            return View(input);
        }

        //[HttpPost]
        public ActionResult Index(Input ip= null)
        {
            ModelState.Clear();

            if (ip == null || ip.ProjectLife == 0 )
            {
                FillViewBag();
                ip = new Input();
                if (TempData["input"] == null)
                {
                    TempData["input"] = ip;

                }
                else
                {
                    ip = TempData["input"] as Input;
                }
                TempData.Keep();
                
                return View(ip);
            }
                
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

        [Obsolete]
        public ActionResult Storage1()
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

        public ActionResult Financial(Input ip = null)
        {
            if (ip == null || ip.County == null)
                return RedirectToAction("Index");
            return RedirectToAction("Index");

          //TODO if financial is not needed, redirect to index
        }

        public ActionResult Storage(Input ip =null)
        {
            if (ip == null || ip.County == null)
                return RedirectToAction("Index");

            if (ip.StorageRequirement == null)
            {
                ip.StorageRequirement = new Storage();
                ModelState.Clear();

            }
            else
            {
                if (!ModelState.IsValid)
                    return View(ip);

            }
               
                
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