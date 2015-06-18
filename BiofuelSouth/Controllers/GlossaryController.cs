using System;
using System.Linq;
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
            ViewBag.AdminToken = (Guid)Session["AdminToken"];
            var glossaries = DataService.GetGlossary();
            return View(glossaries.OrderBy(m=>m.Term));
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
                if (!ModelState.IsValid)
                    return View();

                var result = DataService.VerifyToken(gvm.AdminToken);
                if (!result)
                {
                    var msg = "Token could not be verified. Please try again.";
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

                return RedirectToAction("Index"); 
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


        private GlossaryViewModel ToGlossaryViewModel(Glossary g)
        {
            var gvm = new GlossaryViewModel();
           
            {
                
// ReSharper disable once PossibleInvalidOperationException
                gvm.MId = (Guid) g.Id;
                gvm.Term = g.Term;
                gvm.Description = g.Description;
                gvm.Source = g.Source;
                gvm.Keywords = g.Keywords;
            }
            gvm.AdminToken = (Guid)Session["AdminToken"];
            

            return gvm;
        }

        private Glossary ToGlossary(GlossaryViewModel gvm)
        {
            var gm = new Glossary();
            if (gvm.MId == null)
            {
                gm.Id = Guid.NewGuid();
            }
            else
            {
                gm.Id = gvm.MId; 
            }
            gm.Term = gvm.Term;
            gm.Description = gvm.Description;
            gm.Source = gvm.Source;
            gm.Counter = 0;
            gm.Keywords = gvm.Keywords;
            gm.IsDirty = 1;
            gm.CreatedBy = gvm.AdminToken.ToString();


            return gm;
        }


        // GET: Glossary/Edit/5
        [HttpGet]
        public ActionResult Edit(Guid id, Guid adminToken)
        {
            if (DataService.VerifyToken(adminToken))
            {
                var g = DataService.GetGlossaryById(id);
                var gvm = ToGlossaryViewModel(g);
                return View(gvm);
                
            }
            ViewBag.ErrorMessage = "Token could not be verified. Please try again.";
            return RedirectToAction("Glossary", "Admin");
            
        }

        // POST: Glossary/Edit/5
        [HttpPost]
        public ActionResult Edit(GlossaryViewModel gvm)
        {
            if (!ModelState.IsValid)
                return View(gvm);
            
            if (DataService.VerifyToken(gvm.AdminToken))
            {


                try
                {
                    DataService.UpdateGlossary(gvm);
                    // TODO: Add update logic here

                    return RedirectToAction("Index");
                }
                catch
                {
                    return View();
                }
            }
            return View("Error", (Object) "Invalid Admin Token.");
        }

        // GET: Glossary/Delete/5
        [Obsolete]
        public ActionResult NDelete(String term, Guid adminToken)
        {
            if (DataService.VerifyToken(adminToken))
            {
                DataService.DeleteGlossary(term);
            }

            return RedirectToAction("Index");
            
        }

        // GET: Glossary/Delete/5
        public ActionResult Delete(Guid id, Guid adminToken)
        {
            if (DataService.VerifyToken(adminToken))
            {
                DataService.DeleteGlossary(id);
            }

            return RedirectToAction("Index");

        }

    }
}
