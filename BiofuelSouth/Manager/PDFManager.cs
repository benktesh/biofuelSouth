using System;
using System.Diagnostics;
using BiofuelSouth.ViewModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using PdfSharp.Drawing;
using PdfSharp.Pdf;

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Xml;
// using System.Xml.XPath;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.Rendering;
using System.Diagnostics;
using System.Data;
using System.Text;

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

    public class PDFform
    {
        ResultViewModel rvm { get; set; }
        public PDFform(ResultViewModel rv)
        {
            rvm = rv;
            dt = GetTable();

        }

        private DataTable dt;
        Document document;

        TextFrame addressFrame;

        private String path;

        /// <summary>
        /// The table of the MigraDoc document that contains the invoice items.
        /// </summary>
        MigraDoc.DocumentObjectModel.Tables.Table table;



        /// <summary>
        /// This example method generates a DataTable.
        /// </summary>
        private DataTable GetTable()
        {
            // Here we create a DataTable with four columns.
            DataTable table = new DataTable();
            table.Columns.Add("Dosage", typeof(int));
            table.Columns.Add("Drug", typeof(string));
            table.Columns.Add("Patient", typeof(string));
            table.Columns.Add("Date", typeof(DateTime));

            // Here we add five DataRows.
            table.Rows.Add(25, "Indocin", "David", DateTime.Now);
            table.Rows.Add(50, "Enebrel", "Sam", DateTime.Now);
            table.Rows.Add(10, "Hydralazine", "Christoff", DateTime.Now);
            table.Rows.Add(21, "Combivent", "Janet", DateTime.Now);
            table.Rows.Add(100, "Dilantin", "Melanie", DateTime.Now);
            return table;
        }

        /// <summary>
        /// Creates the invoice document.
        /// </summary>
        public Document CreateDocument()
        {
            // Create a new MigraDoc document
            this.document = new Document();
            this.document.Info.Title = "DSS Results";
            this.document.Info.Subject = "Report";
            this.document.Info.Author = "Biomass Decision Support System";

            DefineStyles();
            CreateResultSummaryPage();
           


            return this.document;
        }

        /// <summary>
        /// Defines the styles used to format the MigraDoc document.
        /// </summary>
        void DefineStyles()
        {
            // Get the predefined style Normal.
            MigraDoc.DocumentObjectModel.Style style = this.document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Verdana";

            style = this.document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = this.document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called Table based on style Normal
            style = this.document.Styles.AddStyle("Table", "Normal");
            style.Font.Name = "Verdana";
            style.Font.Name = "Times New Roman";
            style.Font.Size = 9;

            // Create a new style called Reference based on style Normal
            style = this.document.Styles.AddStyle("Reference", "Normal");
            style.ParagraphFormat.SpaceBefore = "5mm";
            style.ParagraphFormat.SpaceAfter = "5mm";
            style.ParagraphFormat.TabStops.AddTabStop("16cm", TabAlignment.Right);
        }

        /// <summary>
        /// Creates the static parts of the invoice.
        /// </summary>

        void CreatePage()
        {
            // Each MigraDoc document needs at least one section.
            var section = this.document.AddSection();

            // Put a logo in the header
            var image = section.AddImage(path);

            image.Top = ShapePosition.Top;
            image.Left = ShapePosition.Left;
            image.WrapFormat.Style = WrapStyle.Through;

            // Create footer
            Paragraph paragraph = section.Footers.Primary.AddParagraph();
            paragraph.AddText("Health And Social Services.");
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Create the text frame for the address
            this.addressFrame = section.AddTextFrame();
            this.addressFrame.Height = "3.0cm";
            this.addressFrame.Width = "7.0cm";
            this.addressFrame.Left = ShapePosition.Left;
            this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            this.addressFrame.Top = "5.0cm";
            this.addressFrame.RelativeVertical = RelativeVertical.Page;


            // Add the print date field
            paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "6cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Result Summary", TextFormat.Bold);
            paragraph.AddTab();
            paragraph.AddText("Date, ");
            paragraph.AddDateField("MM.dd.yyyy");


            // Put sender in address frame
            paragraph = this.addressFrame.AddParagraph("Some info");
            paragraph.Format.Font.Name = "Times New Roman";
            paragraph.Format.Font.Size = 7;
            paragraph.Format.SpaceAfter = 3;

            // Create the item table
            this.table = section.AddTable();
            this.table.Style = "Table";
            table.Borders.Color = TableBorder;
            this.table.Borders.Width = 0.25;
            this.table.Borders.Left.Width = 0.5;
            this.table.Borders.Right.Width = 0.5;
            this.table.Rows.LeftIndent = 0;


            // Before you can add a row, you must define the columns
            Column column;
            foreach (DataColumn col in dt.Columns)
            {

                column = this.table.AddColumn(Unit.FromCentimeter(3));
                column.Format.Alignment = ParagraphAlignment.Center;

            }

            // Create the header of the table
            Row row = table.AddRow();
            row.HeadingFormat = true;
            row.Format.Alignment = ParagraphAlignment.Center;
            row.Format.Font.Bold = true;
            row.Shading.Color = TableBlue;


            for (int i = 0; i < dt.Columns.Count; i++)
            {

                row.Cells[i].AddParagraph(dt.Columns[i].ColumnName);
                row.Cells[i].Format.Font.Bold = false;
                row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
                row.Cells[i].VerticalAlignment = VerticalAlignment.Bottom;

            }

            this.table.SetEdge(0, 0, dt.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty);


        }


        void CreateResultSummaryPage()
        {
            // Each MigraDoc document needs at least one section.
            var section = this.document.AddSection();

            // Add the print date field
            var paragraph = section.AddParagraph();
            paragraph.Format.SpaceBefore = "0cm";
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Biomass Decision Support System");
            paragraph.AddLineBreak();
            var cropMsg = string.Format("Results for {0} in {1}, {2}", rvm.CropType, rvm.CountyName, rvm.StateName);
            paragraph.AddFormattedText(cropMsg, TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText("Date: ");
            paragraph.AddDateField("MM/dd/yyyy");


            // Put a logo in the header
            //var image = section.AddImage(path);

            //image.Top = ShapePosition.Top;
            //image.Left = ShapePosition.Left;
            //image.WrapFormat.Style = WrapStyle.Through;



            //// Create the text frame for the address
            //this.addressFrame = section.AddTextFrame();
            //this.addressFrame.Height = "6.0cm";
            //this.addressFrame.Width = "14.0cm";
            //this.addressFrame.Left = ShapePosition.Left;
            //this.addressFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            //this.addressFrame.Top = "5.0cm";
            //this.addressFrame.RelativeVertical = RelativeVertical.Page;

            //// Put sender in address frame
            //paragraph = this.addressFrame.AddParagraph("Project Location");
            //paragraph.Format.Font.Name = "Times New Roman";
            //paragraph.Format.Font.Size = 7;
            //paragraph.Format.SpaceAfter = 3;

            paragraph = this.document.LastSection.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.AddFormattedText("Summary Overview", TextFormat.Bold);
            paragraph.AddLineBreak();
             
           var summary =  string.Format(

                "Growing of {0} for a duration of {1} year(s) over an area of {2} in {3}, {4} is expected to produce an estimated {5} tons of biomass annually.", 
                rvm.CropType, rvm.ProjectLife, rvm.ProjectSize, rvm.CountyName, rvm.StateName, rvm.AnnualProduction); 
            paragraph.AddText(summary);

            paragraph.AddText("Growing of ");
            paragraph.AddFormattedText(rvm.CropType.ToString(), TextFormat.Bold);

            paragraph.AddText(" for a duration of ");
            paragraph.AddFormattedText(rvm.ProjectLife.ToString(), TextFormat.Bold);


            paragraph.AddText(" year(s) over an area of ");
            paragraph.AddFormattedText(rvm.ProjectSize, TextFormat.Bold);

            paragraph.AddText(" in ");
            paragraph.AddFormattedText(rvm.CountyName, TextFormat.Bold);

            paragraph.AddText(", ");
            paragraph.AddFormattedText(rvm.StateName, TextFormat.Bold);

            paragraph.AddText(" is expected to produce an estimated ");
            paragraph.AddFormattedText(rvm.AnnualProduction, TextFormat.Bold);

            paragraph.AddText(" tons of biomass annually."); 
            

            paragraph = section.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.AddText("The production comes at an expected annual cost of ");
            paragraph.AddFormattedText(rvm.AnnualCost, TextFormat.Bold);
            paragraph.AddText(" and results in an annual revenue of ");
            paragraph.AddFormattedText(rvm.AnnualRevenue, TextFormat.Bold);
            paragraph.AddText(".");

            paragraph = section.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.AddText("The ");
            var h = paragraph.AddHyperlink("/Home/Search?term=NPV", HyperlinkType.Url);
            h.AddFormattedText("net present value (NPV) ", TextFormat.Underline);
            paragraph.AddText("of the project is estimated to be ");
            paragraph.AddFormattedText(rvm.NPV.ToString("C0"), TextFormat.Bold);
            paragraph.AddText(" at assumed prevailing interest rate of ");
            paragraph.AddFormattedText(rvm.InterestRate.ToString("P"), TextFormat.Bold);  


            paragraph = section.AddParagraph();
            paragraph.Style = "Reference";
            paragraph.Format.Alignment = ParagraphAlignment.Center;
            var cropImage = paragraph.AddImage(System.Web.HttpContext.Current.Server.MapPath(rvm.ImageUrl.Item3));
            cropImage.Width = Unit.FromCentimeter(10);
            cropImage.LockAspectRatio = true; 
            
            paragraph.AddLineBreak();
            paragraph.AddFormattedText(rvm.ImageUrl.Item2, TextFormat.Bold);



            //Row row1;
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    row1 = this.table.AddRow();


            //    row1.TopPadding = 1.5;


            //    for (int j = 0; j < dt.Columns.Count; j++)
            //    {
            //        row1.Cells[j].Shading.Color = TableGray;
            //        row1.Cells[j].VerticalAlignment = VerticalAlignment.Center;
            //        row1.Cells[j].Format.Alignment = ParagraphAlignment.Left;
            //        row1.Cells[j].Format.FirstLineIndent = 1;
            //        row1.Cells[j].AddParagraph(dt.Rows[i][j].ToString());
            //        this.table.SetEdge(0, this.table.Rows.Count - 2, dt.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75);
            //    }
            //}


        
            paragraph = this.document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.Format.Borders.Width = 0.75;
            paragraph.Format.Borders.Distance = 3;
            paragraph.Format.Borders.Color = TableBorder;
            paragraph.Format.Shading.Color = TableGray;
            paragraph.Format.Font.Color = Color.Parse("red");

            paragraph.AddFormattedText("Disclaimer", TextFormat.Bold);
            paragraph.AddLineBreak();
            paragraph.AddText("The Biomass Decision Support System (BDSS) is provided on an 'as is' basis. The University of Tennessee and the developer of BDSS make no warranty that any part of BDSS and including the results generated by this website is suitable for any particular purpose or is error - free.Use of the BDSS, its result and the content of this website at user's sole risk.");
            
            // Create footer
            paragraph = section.Footers.Primary.AddParagraph();
            var footerMsg = string.Format("{0} in {1}, {2}", rvm.CropType, rvm.CountyName, rvm.StateName);
            paragraph.AddText(footerMsg);
            paragraph.Format.Font.Size = 9;
            paragraph.Format.Alignment = ParagraphAlignment.Center;

            // Add the notes paragraph
           


        }

        /// <summary>
        /// Creates the dynamic parts of the invoice.
        /// </summary>
        void FillContent()
        {
            // Fill address in address text frame

            Paragraph paragraph = this.addressFrame.AddParagraph();
            paragraph.AddText("Dr. Anwar Ali");
            paragraph.AddLineBreak();
            paragraph.AddText("Health And Social Services ");
            paragraph.AddLineBreak();
            paragraph.AddText("Karachi");

            Row row1;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                row1 = this.table.AddRow();


                row1.TopPadding = 1.5;


                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    row1.Cells[j].Shading.Color = TableGray;
                    row1.Cells[j].VerticalAlignment = VerticalAlignment.Center;
                    row1.Cells[j].Format.Alignment = ParagraphAlignment.Left;
                    row1.Cells[j].Format.FirstLineIndent = 1;
                    row1.Cells[j].AddParagraph(dt.Rows[i][j].ToString());
                    this.table.SetEdge(0, this.table.Rows.Count - 2, dt.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75);
                }
            }


            // Add the notes paragraph
            paragraph = this.document.LastSection.AddParagraph();
            paragraph.Format.SpaceBefore = "1cm";
            paragraph.Format.Borders.Width = 0.75;
            paragraph.Format.Borders.Distance = 3;
            paragraph.Format.Borders.Color = TableBorder;
            paragraph.Format.Shading.Color = TableGray;

            paragraph.AddText("Note: For any complain please contact us in 24 hours from the issuance of bill");


        }

        // Some pre-defined colors
#if true
        // RGB colors
        readonly static Color TableBorder = new Color(81, 125, 192);
        readonly static Color TableBlue = new Color(235, 240, 249);
        readonly static Color TableGray = new Color(242, 242, 242);
#else
    // CMYK colors
    readonly static Color tableBorder = Color.FromCmyk(100, 50, 0, 30);
    readonly static Color tableBlue = Color.FromCmyk(0, 80, 50, 30);
    readonly static Color tableGray = Color.FromCmyk(30, 0, 0, 0, 100);
#endif
    }
}






/*

    Use Sample
    
    public ActionResult MakePDF()
        {
            var document = (new PDFform()).CreateDocument();

            // Create a renderer for the MigraDoc document.
            const bool unicode = false; //A flag indicating whether to create a Unicode PDF or a WinAnsi PDF file.
            const PdfFontEmbedding embedding = PdfFontEmbedding.Always; // An enum indicating whether to embed fonts or not.

            PdfDocumentRenderer pdfRenderer = new PdfDocumentRenderer( unicode, embedding );
            // Associate the MigraDoc document with a renderer
            pdfRenderer.Document = document;
           // Layout and render document to PDF
            pdfRenderer.RenderDocument();
            var pdf = pdfRenderer.PdfDocument;


            using ( MemoryStream stream = new MemoryStream() )
            {
                pdf.Save(stream, false);
                return File( stream.ToArray(), "application/pdf" );
            }


        }

    */


