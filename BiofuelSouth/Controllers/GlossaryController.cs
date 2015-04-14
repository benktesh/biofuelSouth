using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BiofuelSouth.Models;
using BiofuelSouth.Services;
using BiofuelSouth.ViewModels;

namespace BiofuelSouth.Controllers
{
    public class GlossaryController : Controller
    {
        // GET: Glossary
        public ActionResult Index()
        {
            return View();
        }

        // GET: Glossary/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Glossary/Create
        public ActionResult Create()
        {
            var adminToken = (Guid) Session["AdminToken"];
            var gvm = new GlossaryViewModel();
            gvm.AdminToken = adminToken; 
            return View(gvm);
        }

        // POST: Glossary/Create
        [HttpPost]
        public ActionResult Create(GlossaryViewModel gvm)
        {
            try
            {
                var result = DataService.VerifyToken(gvm.AdminToken);
                if (!result)
                {
                    string msg = "Token could not be verified. Please try again.";
                    return View("Error", (object) msg);
                }

                //TODO - do some sanitization for input data
                var gm = new Glossary();
                gm.Term = gvm.Term;
                gm.Description = gvm.Description;
                gm.Source = gvm.Source;
                gm.Counter = 0;
                gm.Keywords = gvm.Keywords;
                gm.IsDirty = 1;
                gm.CreatedBy = gvm.AdminToken.ToString();

                DataService.SaveGlossary(gm);
                // TODO: Add insert logic here

                return RedirectToAction("Index", "Home");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                
                 return View("Error", (Object) ex.Message);
            }
            catch (Exception ex)
            {
                return View("Error", (Object) ex.StackTrace);
            }
        }

        // GET: Glossary/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Glossary/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Glossary/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Glossary/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
