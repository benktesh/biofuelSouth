using System.Data.Entity;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using BiofuelSouth.Models;

namespace BiofuelSouth.Controllers
{
    public class AdminController : Controller
    {
        private DatabaseContext db = new DatabaseContext();

        // GET: /Admin/
        public async Task<ActionResult> Index()
        {
            return View(await db.Productivities.ToListAsync());
        }

        // GET: /Admin/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productivity productivity = await db.Productivities.FindAsync(id);
            if (productivity == null)
            {
                return HttpNotFound();
            }
            return View(productivity);
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
        public async Task<ActionResult> Create([Bind(Include="Id,CountyId,CropType,Yield")] Productivity productivity)
        {
            if (ModelState.IsValid)
            {
                db.Productivities.Add(productivity);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(productivity);
        }

        // GET: /Admin/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productivity productivity = await db.Productivities.FindAsync(id);
            if (productivity == null)
            {
                return HttpNotFound();
            }
            return View(productivity);
        }

        // POST: /Admin/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include="Id,CountyId,CropType,Yield")] Productivity productivity)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productivity).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(productivity);
        }

        // GET: /Admin/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Productivity productivity = await db.Productivities.FindAsync(id);
            if (productivity == null)
            {
                return HttpNotFound();
            }
            return View(productivity);
        }

        // POST: /Admin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Productivity productivity = await db.Productivities.FindAsync(id);
            db.Productivities.Remove(productivity);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
