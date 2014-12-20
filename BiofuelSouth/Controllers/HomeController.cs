using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BiofuelSouth.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {

            return View();
            //Response.Redirect("~/index.html");
            //RedirectToAction("index", "Input");
            //RedirectToAction("Index", "Input");
        }

        public ActionResult GetSotfwareLog()
        {
            return Redirect("https://docs.google.com/spreadsheets/d/1tW4cMUo6uoi6o9ZsvCx8myMz5J7CTEOC_YOzdYPKRQ0/edit?usp=sharing");
        }

        public ActionResult Help()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return Redirect("~/contact.html");
        }

        public ActionResult Glossary(String term)
        {
            return null;
        }
    }
}