using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using BiofuelSouth.Models;
using BiofuelSouth.Models.Entity;
using BiofuelSouth.Services;

namespace BiofuelSouth.Controllers
{
    public class AdminController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        //http://localhost:51444/admin/glossary?adminToken=1Afe36fa-e1f3-406b-9c73-914ec23ec2be

        
        
        public ActionResult Glossary(Guid? adminToken)
        {
            if (adminToken == null)
            {
                ViewBag.AlertMessage = "Please enter admin token.";
                //return View("Token");
            }

            var result = DataService.VerifyToken(adminToken);
           
            if (result)
            {
                Session["AdminToken"] = adminToken; 
                return RedirectToAction("Index", "Glossary");
            }
            var msg = "Token could not be verified. Please try again.";
            return View("Error", (object) msg);
        }
        // GET: /Admin/
        public async Task<ActionResult> Index()
        {
            return View((List<ProductivityEntity>) await db.Productivities.ToListAsync());
        }

        
        // GET: /Admin/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Admin/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include="Id,CountyId,CropType,Yield")] ProductivityEntity productivity)
        {
            throw new NotImplementedException();
            //if (ModelState.IsValid)
            //{
            //    db.Productivities.Add(productivity);
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}

            //return View(productivity);
        }

        // GET: /Admin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            throw new NotImplementedException();
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //var productivity = await db.Productivities.FindAsync(id);
            //if (productivity == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(productivity);
        }

        // POST: /Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,CountyId,CropType,Yield")] ProductivityEntity productivity)
        {
            throw new NotImplementedException();
            //if (ModelState.IsValid)
            //{
            //    db.Entry(productivity).State = EntityState.Modified;
            //    await db.SaveChangesAsync();
            //    return RedirectToAction("Index");
            //}
            //return View(productivity);
        }

        // GET: /Admin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            throw new NotImplementedException();
            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //var productivity = await db.Productivities.FindAsync(id);
            //if (productivity == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(productivity);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            throw new NotImplementedException();
            //var productivity = await db.Productivities.FindAsync(id);
            //db.Productivities.Remove(productivity);
            //await db.SaveChangesAsync();
            //return RedirectToAction("Index");
        }


        public ActionResult Error(String msg)
        {
            return View((object) msg);
            
        }

        public ActionResult Dashboard(Guid? adminToken = null)
        {
            if (adminToken != null)
            {
                if (DataService.VerifyToken(adminToken))
                {
                    Session["AdminToken"] = adminToken;
                }
                else
                {
                    Session["AdminToken"] = null;
                }
                
            }
            // ReSharper disable once Mvc.ViewNotResolved
            return View();
        }

    }
}
