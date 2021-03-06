﻿using System.IO;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Enum;
using BiofuelSouth.Manager;
using BiofuelSouth.Models;
using BiofuelSouth.ViewModels;
using MigraDoc.Rendering;

// ReSharper disable InconsistentNaming


namespace BiofuelSouth.Controllers
{
	// ReSharper disable once InconsistentNaming
    public class DSSController : Controller
    {
		
        [HttpGet]
        public ActionResult Index()
        {
            Input ip = new Input();
	        PopulateHelpers(ip);
			InputSet( ip );
			return RedirectToAction("General");
        }

        [HttpPost]
        public ActionResult Index(Input ip)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }

            if (ip == null)
            {
	            return RedirectToAction("Index");
            }
            
            PopulateHelpers(ip);
	        InputSet(ip);

            ModelState.Clear();
            return RedirectToAction("General");
        }


        [HttpGet]
        public ActionResult General()
        {
            var ip = InputGet();
			
            PopulateHelpers(ip);
	        InputSet(ip);
            return View(ip.General);
        }

        [HttpPost]
        public ActionResult General(General general)
        {

            if (!ModelState.IsValid)
            {
                PopulateHelpers(general);
                return View(general);
            }

            ModelState.Clear();
            var ip = InputGet();
            if (ip == null)
            {
                return RedirectToAction("General");
            }
            
            ip.General = general;
			InputSet( ip );
			return RedirectToAction("GetProductionCost");

        }


        private Input InputGet()
        {
            var ip = Session["Input"] as Input;
	        if (ip == null)
	        {
				ip = new Input();
				InputSet( ip );
		        
	        }
	        return ip;  
        }

        private void InputSet(Input input = null)
        {
            Session["Input"] = input;
	        InputGet();
        }

        [HttpGet]
        public ActionResult Storage()
        {
            var ip = InputGet();
            if (ip == null)
            {
                return RedirectToAction("Index");
            }
            var storage = ip.Storage;
            if (storage == null)
            {
                storage = new Storage();
            }

            storage.CurrentStep = WizardStep.Storage;
            storage.PreviousAction = "GetProductionCost";

           

            

            if (ip.General.Category == CropType.Poplar || ip.General.Category == CropType.Pine || ip.General.Category == CropType.Willow)
            {
                storage.RequireStorage = false;
                ip.Storage = storage;
	            InputSet(ip);
                return RedirectToAction("Financial");
            }

            ip.Storage = storage;
			InputSet( ip );
			return View(storage);
        }

        [HttpPost]
        public ActionResult Storage(Storage storage)
        {
	        var ip = InputGet(); 
            if (ip == null || storage == null) //if session input is null or storage is null, return to index
            {
                return RedirectToAction("Index");
            }

            if (!storage.RequireStorage.GetValueOrDefault()) //if storage is indicated as not required, clear the model state;  
            {
                ModelState.Clear();
            }
           

            if (!ModelState.IsValid)
            {
                return View("Storage", storage);
            }


            if (ModelState.IsValid && ip.General.Category == CropType.Poplar || ip.General.Category == CropType.Pine || ip.General.Category == CropType.Willow)
            {
                storage.RequireStorage = false;
            }

            ip.Storage = storage;
	        InputSet(ip); 

            return RedirectToAction("Financial");
            
        }

        [HttpGet]
        public ActionResult GetProductionCost()
        {
            var model = GetProductionCostViewModel(); 
            return View("_productionCost", model);
        }

        public ActionResult NewDSS()
        {
			Input ip = new Input();
	        InputSet(ip);
            return RedirectToAction("General");
        }

    
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateProductionCost(ProductionCostViewModel model)
        {
			
	        var input = InputGet(); 
            if ( input == null)
            {
                return RedirectToAction("General");
            }

            if (ModelState.IsValid)
            {
                model.UseCustom = true;
                if (model.CropType == CropType.Miscanthus || model.CropType == CropType.Switchgrass)
                {
                    model.UseCustom = false;
                }
                  
                input.ProductionCost = model;
	            InputSet( input );

                if (input.General.Category == CropType.Miscanthus || input.General.Category == CropType.Switchgrass)
                    return RedirectToAction("Storage");
                return RedirectToAction("Financial");
            }

            return View("_productionCost", model);
        }

        private ProductionCostViewModel GetProductionCostViewModel()
        {
            ProductionCostManager pcm = new ProductionCostManager();

	        var input = InputGet();
            return pcm.GetProductionCost(new ProductionCostViewModel { CropType = input.General.Category, County = input.General.County, UseCustom = true});
        }

        [HttpGet]
        public ActionResult Financial()
        {
            var ip = InputGet();
            if (ip == null)
            {
                return RedirectToAction("Index");
            }

            var financial = ip.Financial ?? new Financial();

            financial.CurrentStep = WizardStep.Financial;
            
            if (ip.General.Category == CropType.Miscanthus || ip.General.Category == CropType.Switchgrass)
            {

                financial.PreviousAction = "Storage";
            }
            else
            {
                financial.PreviousAction = "GetProductionCost";
            }

            ip.Financial = financial;
	        InputSet(ip);
           
            ModelState.Clear();
            return View(financial);

        }

        [HttpPost]
        public ActionResult Financial(Financial financial)
        {
	        var ip = InputGet();
            if (ip == null || financial == null)
            {
                return RedirectToAction("Index");
            }


            if (financial.RequireFinance.HasValue && !financial.RequireFinance.Value)
            {
                ModelState.Clear();
            }

            if (ModelState.IsValid)
            {
                financial.CurrentStep = WizardStep.Result;
                ip.Financial = financial;
	            InputSet(ip);
                return RedirectToAction("Results");
            }

            return View("Financial", financial);
  }

        [HttpGet]
        public ActionResult GetAlternative(CropType cropType)
        {
            return null;  
        }


        public ActionResult Results()
        {
            var input = InputGet();

	        Session["Prestine"] = input; 
            if (input == null)
            {
                return RedirectToAction("General");
            }
			//var rm = new ResultManager((Input) input.Clone());
			//  var vm = rm.GetResultViewModel();


			var vms = Simulator.GetViewModels( (Input)input.Clone() );

			var temp = Session["Prestine"] as Input;
			var vm = vms.FirstOrDefault(m => temp != null && m.CropType == temp.General.Category);
            Session["CurrentResult"] = vm;
            return View("TabbedResult", vm);
        }

        private Input GetDefaultInput()
        {
            var ip = new Input();
            ip.General.Category = CropType.Willow;
            ip.General.ProjectLife = 10;
            ip.General.ProjectSize = 100;
            ip.General.State = "AL";
            ip.General.County = "01001";
            ip.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(ip.General.Category);
            ip.General.LandCost = 70;

			ProductionCostManager pcm = new ProductionCostManager();
			ip.ProductionCost = pcm.GetProductionCost( new ProductionCostViewModel { CropType = ip.General.Category, County = ip.General.County, UseCustom = true } );
			
            return ip;
        }

		[HttpGet]
        public ActionResult TestTabbedResult()
		{
		    var input = GetDefaultInput();

            Session["Prestine"] = input;
           
            var vms = Simulator.GetViewModels((Input)input.Clone());

            var temp = Session["Prestine"] as Input;
            var vm = vms.FirstOrDefault(m => temp != null && m.CropType == temp.General.Category);

		    Session["CurrentResult"] = vm; 
            return View("TabbedResult", vm);


        }

        private void PopulateHelpers(Input model)
        {

            model.General.CountyList = Constants.GetCountySelectList(model.General.State);
            model.General.StateList = Constants.GetState();
            model.General.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.General.Category);
        }

        private void PopulateHelpers(General model)
        {
            model.CountyList = Constants.GetCountySelectList(model.State);
            model.StateList = Constants.GetState();
            model.BiomassPriceAtFarmGate = Constants.GetFarmGatePrice(model.Category);
        }

        [HttpGet]
        public ActionResult GenerateReport()
        {

           var vm = (ResultViewModel) Session["CurrentResult"];
            if (vm == null)
                RedirectToAction("Results");
            
            var document = (new PDFform(vm)).CreateDocument();

            // Create a renderer for the MigraDoc document.
            //const bool unicode = false; //A flag indicating whether to create a Unicode PDF or a WinAnsi PDF file.
            //const PdfFontEmbedding embedding = PdfFontEmbedding.Always; // An enum indicating whether to embed fonts or not.

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer();
            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;
            // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            var pdf = pdfRenderer.PdfDocument;

            using (MemoryStream stream = new MemoryStream())
            {
                pdf.Save(stream, false);
                return File(stream.ToArray(), "application/pdf");
            }
        }
    }


}