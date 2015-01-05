using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Models;
using BiofuelSouth.Services;

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

        [HttpGet]
        public ActionResult FeedBack()
        {
            FeedBack fb = new FeedBack();
            //Save feedback
            //Send email to client acknowledging receipt of the feedback
            //Send email to Resource about the new Feedback
            return View(fb);
        }

        [HttpPost]
        public ActionResult FeedBack(FeedBack fb)
        {
            fb.Date = DateTime.UtcNow;
            DataService.SaveFeedback(fb);
            //Save feedback
            //Send email to client acknowledging receipt of the feedback
            //Send email to Resource about the new Feedback
            return View(fb);
        }

        public ActionResult Help()
        {

            return View();
        }

        public ActionResult Contact()
        {
            return Redirect("~/contact.html");
        }

        public ActionResult Search(String term="")
        {
            ViewData["glossary"] = DataService.GetGlossary();
            ViewData["term"] = "";
            IList<Glossary> x = null; 
            if (term != null)
            {
                x = DataService.Search(term);
                ViewData["term"] = term;
            }
            return View(x);
            
        }

        public void Glossary()
        {
            Search();
        }
    }
}