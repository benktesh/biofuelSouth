using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using BiofuelSouth.ViewModels;
using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

namespace BiofuelSouth.Manager
{
	class PDFManager
	{
		private static PDFManager instance;

		private PDFManager() { }

		public static PDFManager GetInstance
		{
			get
			{
				if ( instance == null )
				{
					instance = new PDFManager();
				}
				return instance;
			}
		}

		public static PdfDocument  GetPDF(ResultViewModel rvm)
		{
			var Title = String.Format("{0}-{1},{2}", rvm.CropType, rvm.CountyName, rvm.StateName);

			PdfDocument pdf = new PdfDocument();
			pdf.Info.Title = Title;
			PdfPage pdfPage = pdf.AddPage();
			XGraphics graph = XGraphics.FromPdfPage( pdfPage );
			XFont font = new XFont( "Verdana", 20, XFontStyle.Bold );
			graph.DrawString( "This is my first PDF document", font, XBrushes.Black, new XRect( 0, 0, pdfPage.Width.Point, pdfPage.Height.Point ), XStringFormats.Center );
			string pdfFilename = "firstpage.pdf";
			pdf.Save( pdfFilename );
			Process.Start( pdfFilename );

			return pdf; 
		}

		//public PdfDocument GenerateReport( ReportPdfInput input )
		//{
		//	var document = CreateDocument( input );
		//	var renderer = new PdfDocumentRenderer( true,
		//		PdfSharp.Pdf.PdfFontEmbedding.Always );
		//	renderer.Document = document;
		//	renderer.RenderDocument();

		//	return renderer.PdfDocument;
		//}

		//private Document CreateDocument( ReportPdfInput input )
		//{
		//	//Creates a Document and puts content into it
		//}


	}
}
