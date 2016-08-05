using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web.Mvc;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.Models.Entity;
using BiofuelSouth.Services;
using log4net;
using GlossaryEntity = BiofuelSouth.Models.Entity.GlossaryEntity;

namespace BiofuelSouth.Controllers
{
    public class HomeController : Controller
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        // GET: Home
        public ActionResult Index()
        {
            Log.Info(Request.UserHostAddress);
	        ViewData["glossary"] = GetTopSearches();


			return View();

        }

        public ActionResult AskExpert()
        {

            return View();
        }

        public ActionResult GetSotfwareLog()
        {
            return Redirect("https://docs.google.com/spreadsheets/d/1tW4cMUo6uoi6o9ZsvCx8myMz5J7CTEOC_YOzdYPKRQ0/edit?usp=sharing");
        }

        [HttpGet]
        public ActionResult FeedBack()
        {
            var fb = new FeedBackEntity();
            //Save feedback
            //Send email to client acknowledging receipt of the feedback
            //Send email to Resource about the new Feedback
            return View(fb);
        }

        [HttpPost]
        public ActionResult FeedBack(FeedBackEntity fb)
        {
            fb.Date = DateTime.UtcNow;
            DataService.SaveFeedback(fb);

            var msgBody = new StringBuilder();

            msgBody.Append("Dear " + fb.Name + "," + "\n");
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

        public ActionResult Factsheet()
        {
            var model = new FactsheetViewModel();
	        
            return View(model);
        }

        
		public ActionResult GetFact( CropType cropType )
		{
			var model = DataService.GetFactSheet( cropType );
			//var model = DataService.GetFactsheetViewModel( cropType );
			if (Request.IsAjaxRequest())
			{
				return PartialView("_cropFact", model);
			}
			else
			{
				var fsvm = new FactsheetViewModel {CropType = cropType, FactItem = model};
				return View("Factsheet", fsvm);
			}
			
		}

		public ActionResult GetPdfFact(CropType cropType)
        {

            var fileName = string.Format("{0}.pdf", cropType.ToString()).ToLower();
            var path = Path.Combine(Server.MapPath("~/Files"), fileName);

            if (!System.IO.File.Exists(path))
            {
                return HttpNotFound();
            }

            return File(path, MediaTypeNames.Application.Pdf);

        }

        public ActionResult Contact()
        {
            return FeedBack();
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Search(String term = "")
        {
            ViewData["glossary"] = DataService.GetGlossary().ToList();
            ViewData["term"] = "";
            IList<GlossaryEntity> x = null;
            if (term != null)
            {
                x = DataService.Search(term);
                ViewData["term"] = term;
            }
            return View(x);
        }

	    public IList<GlossaryEntity> GetTopSearches(int count = 20)
	    {
		    var topSearches =  DataService.GetGlossary(count).ToList();
		    return topSearches; 
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

        public ActionResult GetCountyData(string selectedCategory = "ALL")
        {
            IList<CountyEntity> countyList = DataService.GetCountyData(selectedCategory);
            var data = countyList.Select(p => new
            {
                p.Name,
                GeoID = p.GeoId,
                p.CountyCode,
                p.Lat,
                p.Lon
            });

            return(Json(data, JsonRequestBehavior.AllowGet));
        }
    }
}