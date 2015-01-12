using System;
using System.Collections.Generic;
using System.Text;
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

            StringBuilder msgBody = new StringBuilder();

            msgBody.Append("Dear " + fb.Name + "," +"\n");
            msgBody.Append("Thank you for contacting us. Your feedback is important to us. If your feedback (as shown below) requires response from us, you will be contacted by one of our team members.\n\n");
            msgBody.Append("The Biofuel South DSS Team.\n\n" +

            "------------Your Feedback ----------------\n\n" + fb.Message);

            EMailService.SendEmail(msgBody.ToString(), fb.Email, "Feedback received at Biofuel DSS.");
            
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
            return FeedBack();
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

        public JsonResult GetListOfWords(string key)
        {
            var result = DataService.GetAllTerms(key);
            return new JsonResult
            {
                JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                Data = result
            };


        }

        public void Glossary()
        {
            Search();
        }
    }
}