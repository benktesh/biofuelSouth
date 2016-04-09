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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Helpers;
using System.Web.UI.WebControls;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BorderStyle = MigraDoc.DocumentObjectModel.BorderStyle;
using Unit = MigraDoc.DocumentObjectModel.Unit;

namespace BiofuelSouth.Manager
{
	public class PDFform
	{
		ResultViewModel rvm { get; set; }
		public PDFform( ResultViewModel rv )
		{
			rvm = rv;
			dt = GetTable();
			keyValue = null; 

		}

		private DataTable dt;
		Document document;

		private string keyValue { get; set;  }

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
			table.Columns.Add( "Dosage", typeof( int ) );
			table.Columns.Add( "Drug", typeof( string ) );
			table.Columns.Add( "Patient", typeof( string ) );
			table.Columns.Add( "Date", typeof( DateTime ) );

			// Here we add five DataRows.
			table.Rows.Add( 25, "Indocin", "David", DateTime.Now );
			table.Rows.Add( 50, "Enebrel", "Sam", DateTime.Now );
			table.Rows.Add( 10, "Hydralazine", "Christoff", DateTime.Now );
			table.Rows.Add( 21, "Combivent", "Janet", DateTime.Now );
			table.Rows.Add( 100, "Dilantin", "Melanie", DateTime.Now );
			return table;
		}

		/// <summary>
		/// Creates the invoice document.
		/// </summary>
		public Document CreateDocument()
		{
			// Create a new MigraDoc document
			this.document = new Document();
			document.DefaultPageSetup.PageFormat = PageFormat.Letter;

			this.document.Info.Title = "DSS Results";
			this.document.Info.Subject = "Report";
			this.document.Info.Author = "Biomass Decision Support System";

			DefineStyles();
			CreateResultSummaryPage();
			AddHeaderAndFooter();



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
			style.ParagraphFormat.AddTabStop( "16cm", TabAlignment.Right );

			style = this.document.Styles[StyleNames.Footer];
			style.ParagraphFormat.AddTabStop( "8cm", TabAlignment.Center );

			// Create a new style called Table based on style Normal
			style = this.document.Styles.AddStyle( "Table", "Normal" );
			style.Font.Name = "Verdana";
			style.Font.Name = "Times New Roman";
			style.Font.Size = 9;

			// Create a new style called Reference based on style Normal
			style = this.document.Styles.AddStyle( "Reference", "Normal" );
			style.ParagraphFormat.SpaceBefore = "5mm";
			style.ParagraphFormat.SpaceAfter = "5mm";
			style.ParagraphFormat.TabStops.AddTabStop( "16cm", TabAlignment.Right );
		}

		/// <summary>
		/// Creates the static parts of the invoice.
		/// </summary>

		void CreatePage()
		{
			// Each MigraDoc document needs at least one section.
			var section = this.document.AddSection();

			// Put a logo in the header
			var image = section.AddImage( path );

			image.Top = ShapePosition.Top;
			image.Left = ShapePosition.Left;
			image.WrapFormat.Style = WrapStyle.Through;

			// Create footer
			Paragraph paragraph = section.Footers.Primary.AddParagraph();
			paragraph.AddText( "Health And Social Services." );
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
			paragraph.AddFormattedText( "Result Summary", TextFormat.Bold );
			paragraph.AddTab();
			paragraph.AddText( "Date, " );
			paragraph.AddDateField( "MM.dd.yyyy" );


			// Put sender in address frame
			paragraph = this.addressFrame.AddParagraph( "Some info" );
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
			foreach ( DataColumn col in dt.Columns )
			{

				column = this.table.AddColumn( Unit.FromCentimeter( 3 ) );
				column.Format.Alignment = ParagraphAlignment.Center;

			}

			// Create the header of the table
			Row row = table.AddRow();
			row.HeadingFormat = true;
			row.Format.Alignment = ParagraphAlignment.Center;
			row.Format.Font.Bold = true;
			row.Shading.Color = TableBlue;


			for ( int i = 0; i < dt.Columns.Count; i++ )
			{

				row.Cells[i].AddParagraph( dt.Columns[i].ColumnName );
				row.Cells[i].Format.Font.Bold = false;
				row.Cells[i].Format.Alignment = ParagraphAlignment.Left;
				row.Cells[i].VerticalAlignment = VerticalAlignment.Bottom;

			}

			this.table.SetEdge( 0, 0, dt.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75, Color.Empty );


		}


		void CreateResultSummaryPage()
		{
			// Each MigraDoc document needs at least one section.
			var section = this.document.AddSection();


			// Add the print date field
			var paragraph = section.AddParagraph();
			paragraph.Format.SpaceBefore = "0cm";
			paragraph.Style = "Reference";
			paragraph.AddFormattedText( "Biomass Decision Support System" );
			paragraph.AddLineBreak();
			var cropMsg = string.Format( "Results for {0} in {1}, {2}", rvm.CropType, rvm.CountyName, rvm.StateName );
			paragraph.AddFormattedText( cropMsg, TextFormat.Bold );
			paragraph.AddLineBreak();
			paragraph.AddText( "Date: " );
			paragraph.AddDateField( "MM/dd/yyyy" );
            paragraph.Format.Alignment = ParagraphAlignment.Center;


			#region Summary 
			paragraph = this.document.LastSection.AddParagraph();
			paragraph.Style = "Reference";
			var sectionHead = paragraph.AddFormattedText( "Summary", TextFormat.Bold );
			sectionHead.Font.Size = 13;

			paragraph.AddLineBreak();

			//paragraph.AddText( summary );

			paragraph.AddText( "Growing of " );
			paragraph.AddFormattedText( rvm.CropType.ToString(), TextFormat.Bold );

			paragraph.AddText( " for a duration of " );
			paragraph.AddFormattedText( rvm.ProjectLife.ToString(), TextFormat.Bold );


			paragraph.AddText( " year(s) over an area of " );
			paragraph.AddFormattedText( rvm.ProjectSize, TextFormat.Bold );

			paragraph.AddText( " in " );
			paragraph.AddFormattedText( rvm.CountyName, TextFormat.Bold );

			paragraph.AddFormattedText("County, " , TextFormat.Bold);
			paragraph.AddFormattedText( rvm.StateName, TextFormat.Bold );

			paragraph.AddText( " is expected to produce an estimated " );
			paragraph.AddFormattedText( rvm.AnnualProduction, TextFormat.Bold );

			paragraph.AddText( " tons of biomass annually." );

			paragraph = document.LastSection.AddParagraph();
			paragraph.Style = "Reference";
			paragraph.Format.Alignment = ParagraphAlignment.Center;
			var cropImage = paragraph.AddImage( System.Web.HttpContext.Current.Server.MapPath( rvm.ImageUrl.Item3 ) );
			cropImage.Width = Unit.FromCentimeter( 8 );
			cropImage.LockAspectRatio = true;
			paragraph.AddLineBreak();
			var t = paragraph.AddFormattedText( rvm.ImageUrl.Item2);

			var lat = rvm.CountyEntity.Lat;
			var lon = rvm.CountyEntity.Lon;
			var location = string.Format( "center={0},{1}", lat, lon );
			Task<byte[]> result = GetStaticMap( location, "400x300" );

			if ( result != null )
			{
				paragraph = document.LastSection.AddParagraph();
				paragraph.AddLineBreak();
				paragraph.Format.Alignment = ParagraphAlignment.Center;
				var file = result.Result;
				string imgfile = FileFromBytes( file );

				paragraph.AddLineBreak();
				var map = paragraph.AddImage( imgfile );
				map.Height = Unit.FromCentimeter( 6 );
				paragraph.AddLineBreak();
				paragraph.AddFormattedText( "Approximate Location of Assessment Area");
				paragraph.AddLineBreak();
			}


			paragraph = section.AddParagraph();
			paragraph.Style = "Reference";
			paragraph.AddText( "The production comes at an expected annual cost of " );
			paragraph.AddFormattedText( rvm.AnnualCost, TextFormat.Bold );
			paragraph.AddText( " and results in an annual revenue of " );
			paragraph.AddFormattedText( rvm.AnnualRevenue, TextFormat.Bold );
			paragraph.AddText( ". " );

			paragraph.AddText( "The " );
			var h = paragraph.AddHyperlink( "/Home/Search?term=NPV", HyperlinkType.Url );
			h.AddFormattedText( "net present value (NPV) ", TextFormat.Underline );
			paragraph.AddText( "of the project is estimated to be " );
			paragraph.AddFormattedText( rvm.NPV.ToString( "C0" ), TextFormat.Bold );
			paragraph.AddText( " at assumed prevailing interest rate of " );
			paragraph.AddFormattedText( rvm.InterestRate.ToString( "P" ), TextFormat.Bold );
			
			if ( rvm.NPV < 0 )
			{

				paragraph.AddText( "This means there will be a net loss of " );
				var formatted = paragraph.AddFormattedText( rvm.NPV.ToString( "C0" ), TextFormat.Bold );
				formatted.Color = Colors.Red;
			}
			else
			{

				paragraph.AddText( "This means there will be a net profit of " );
				paragraph.AddFormattedText( rvm.NPV.ToString( "C0" ), TextFormat.Bold );
			}
			paragraph.AddText( " from this project during project's life (" );
		    paragraph.AddFormattedText(rvm.ProjectLife.ToString(), TextFormat.Bold);
            paragraph.AddText(" years).");


            paragraph.AddLineBreak();



			//comparision with other crops
			paragraph = document.LastSection.AddParagraph();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			paragraph.AddLineBreak();
			paragraph.AddFormattedText( "Comparison with other crops", TextFormat.Bold );
			paragraph.AddLineBreak();

			var highNPV = rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.HighNpv );
			var lowNPV = rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.LowNpv );

			if ( highNPV != null && highNPV.Crop != rvm.CropType )
			{
				var highNpv = Decimal.Parse( highNPV.ComparisionValue );
				paragraph.AddText( "It is anticipated that growing " );
				paragraph.AddFormattedText( highNPV.Crop.ToString(), TextFormat.Bold );

				if ( highNpv >= 0 )
				{

					paragraph.AddText( " may likely result in the highest NPV with a net profit of " );
					paragraph.AddFormattedText( highNpv.ToString( "C0" ), TextFormat.Bold );
				}
				else
				{
					paragraph.AddText( " may likely result in the highest NPV with a net loss of " );
					var txt = paragraph.AddFormattedText( highNpv.ToString( "C0" ), TextFormat.Bold );
					txt.Color = Colors.Red;
				}
				paragraph.AddText( " among crops considered in this particular anlaysis." );


			}
			else if ( highNPV != null && highNPV.Crop == rvm.CropType )
			{
				paragraph.AddText( "It is anticipated that growing " );
				paragraph.AddFormattedText( highNPV.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may likely result in highest profit compared to other crops." );

			}
			if ( lowNPV != null && lowNPV.Crop != rvm.CropType )
			{
				//paragraph.AddLineBreak();
				paragraph.AddText( "It is anticipated that growing " );
				paragraph.AddFormattedText( lowNPV.Crop.ToString(), TextFormat.Bold );
				var lowNpv = Decimal.Parse( lowNPV.ComparisionValue );
				if ( lowNpv >= 0 )
				{

					paragraph.AddFormattedText( lowNPV.Crop.ToString(), TextFormat.Bold );
					paragraph.AddText( " may likely result in lowest NPV with a net profit of " );
					paragraph.AddFormattedText( lowNpv.ToString( "C0" ), TextFormat.Bold );
				}
				else
				{
					paragraph.AddText( " may likely result in lowest NPV resulting into a net loss of " );
					var text = paragraph.AddFormattedText( lowNpv.ToString( "C0" ), TextFormat.Bold );
					text.Color = Colors.Red;
				}
				paragraph.AddText( "." );
			}
			else if ( lowNPV != null && lowNPV.Crop == rvm.CropType )
			{
				//paragraph.AddLineBreak();
				paragraph.AddText( "Likewise, it is anticipated that growing " );
				paragraph.AddFormattedText( lowNPV.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may result the lowest profit compared to growing other crops." );
			}
			string keyValue = null; 
			if (rvm.ChartKeys.Select(m => m.Key == ChartType.NPVCompare).Any())
			{
				keyValue = rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.NPVCompare ).Value;
				GetImage(keyValue, "Comparision of project NPV ($) across various biofuel crops"); 
			}
			

			#endregion Summary
			//Production Results:
			paragraph = document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			sectionHead = paragraph.AddFormattedText( "Production Results", TextFormat.Bold );
			sectionHead.Font.Size = 13;

			paragraph.AddLineBreak(); 
			paragraph.AddText( "Growing of " );
			paragraph.AddFormattedText( rvm.CropType.ToString(), TextFormat.Bold );
			paragraph.AddText( " for a duration of " );
			paragraph.AddFormattedText( rvm.ProjectLife.ToString(), TextFormat.Bold );
			paragraph.AddText( "  years over an area of " );
			paragraph.AddFormattedText( rvm.ProjectSize.ToString(), TextFormat.Bold );
			paragraph.AddText( "  in " );
			paragraph.AddText( " year(s) over an area of " );
			paragraph.AddFormattedText( rvm.ProjectSize, TextFormat.Bold );

			paragraph.AddText( " in " );
			paragraph.AddFormattedText( rvm.CountyName, TextFormat.Bold );

			paragraph.AddFormattedText( " County, ", TextFormat.Bold );
			paragraph.AddFormattedText( rvm.StateName, TextFormat.Bold );

			paragraph.AddText( " is expected to produce an estimated " );
			paragraph.AddFormattedText( rvm.AnnualProduction, TextFormat.Bold );

			paragraph.AddText( " tons of biomass annually." );

		
			if ( rvm.ChartKeys.Select( m => m.Key == ChartType.Production ).Any() )
            { 
				keyValue = rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.Production ).Value;
				GetImage(keyValue, Constants.ProductionChartCaption( rvm.CropType ) ); 
			}

			#region production comparision
			paragraph = document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			paragraph.AddFormattedText( "Comparison with other crops", TextFormat.Bold );
			var high = rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.HighProduction );
			var low = rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.LowProduction );

			paragraph.AddLineBreak();
			paragraph.AddText( "It is anticipated that growing " );
			paragraph.AddFormattedText( high.Crop.ToString(), TextFormat.Bold );
			paragraph.AddText( " may likely result in the highest production of " );
			paragraph.AddFormattedText( high.ComparisionValue, TextFormat.Bold );

			if ( high != null && high.Crop != rvm.CropType )
			{

				paragraph.AddText( " tons." );
			}

			else if ( high != null && high.Crop == rvm.CropType )
			{
				paragraph.AddText( " tons compared to other crops." );

			}

			paragraph.AddText( "It is anticipated that growing " );
			paragraph.AddFormattedText( low.Crop.ToString(), TextFormat.Bold );
			paragraph.AddText( " may likely result in the highest production of " );
			paragraph.AddFormattedText( low.ComparisionValue, TextFormat.Bold );
			if ( low != null && low.Crop != rvm.CropType )
			{
				paragraph.AddText( " tons." );
			}
			else if ( low != null && low.Crop == rvm.CropType )
			{
				paragraph.AddText( " tons compared to other crops." );
			}

			if ( rvm.ChartKeys.Select( m => m.Key == ChartType.ProductionCompare ).Any() )
			{
				keyValue = rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.ProductionCompare ).Value;
				GetImage( keyValue, Constants.ProductionCompareChartCaption(rvm.CropType) );
			}
			#endregion production comparision

			#region Cost and revenue
			paragraph = document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			sectionHead = paragraph.AddFormattedText( "Cost And Revenue", TextFormat.Bold );
			sectionHead.Font.Size = 13;
			paragraph.AddLineBreak();

			paragraph.AddText( "The production comes at an expected annual cost of " );
			paragraph.AddFormattedText( @rvm.AnnualCost, TextFormat.Bold );

			paragraph.AddText( " and results in an annual revenue of " );
			paragraph.AddFormattedText( @rvm.AnnualRevenue, TextFormat.Bold );

			paragraph.AddText( ". The net present value of the project is estimated to be " );
			paragraph.AddFormattedText( @rvm.NPV.ToString( "C0" ), TextFormat.Bold );

			paragraph.AddText( "at assumed prevailing interest rate of " );
			paragraph.AddFormattedText( @rvm.InterestRate.ToString( "P" ), TextFormat.Bold );
			paragraph.AddText( "." );

			if ( rvm.ChartKeys.Select( m => m.Key == ChartType.CostRevenue ).Any() )
			{
				keyValue = rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.CostRevenue ).Value;
				GetImage( keyValue, "Cost and revenue information" );
			}

			paragraph = document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.AddFormattedText( "Comparison with other crops", TextFormat.Bold );
			paragraph.AddLineBreak();
			var highPrice = rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.HighFarmGatePrice );
			var lowPrice = rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.LowFarmgatePrice );

			paragraph.AddText( "Among the biomass crops considered in this particular analysis, " +
			                   "it is anticipated that growing " );
			paragraph.AddFormattedText( highPrice.Crop.ToString(), TextFormat.Bold );

			if ( highPrice != null && highPrice.Crop != rvm.CropType )
			{
				paragraph.AddText( " may likely result in the highest farmgate price of " );
				paragraph.AddFormattedText( highPrice.ComparisionValue, TextFormat.Bold );
				paragraph.AddText( " per ton." );
			}
			else if ( highPrice != null && highPrice.Crop == rvm.CropType )
			{
				paragraph.AddText( " is the option that results in the highest farmgate price of " );
				paragraph.AddFormattedText( highPrice.ComparisionValue, TextFormat.Bold );
				paragraph.AddText( " per ton compared to other crops." );
			}

			paragraph.AddText( " Likewise, it is anticipated that growing " );
			paragraph.AddFormattedText( @lowPrice.Crop.ToString(), TextFormat.Bold );
			paragraph.AddText( " may likely result in the lowest farmgate price of " );
			paragraph.AddFormattedText( lowPrice.ComparisionValue, TextFormat.Bold );


			if ( lowPrice != null && lowPrice.Crop != rvm.CropType )
			{


				paragraph.AddText( " per ton." );

			}
			else if ( lowPrice != null && lowPrice.Crop == rvm.CropType )
			{
				paragraph.AddText( " per ton compared to growing other crops." );

			}
			paragraph.AddLineBreak(); 

			if ( rvm.ChartKeys.Select( m => m.Key == ChartType.CashFlow ).Any() )
			{
				keyValue = rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.CashFlow ).Value;
				GetImage( keyValue, Constants.CashFlowChartCaption( rvm.CropType ) );


			}

			if ( rvm.ChartKeys.Select( m => m.Key == ChartType.CashFlowCompare ).Any() )
			{
				keyValue = rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.CashFlowCompare ).Value;

				GetImage( keyValue, @Constants.CashFlowCompareChartCaption( rvm.CropType ) );
			}

			#endregion


			RenderInputAndAssumption();

			RenderMoreInformation();

			RenderDisclaimer();
		}

		private void RenderInputAndAssumption()
		{
			Paragraph paragraph;
			FormattedText sectionHead;
			document.LastSection.AddPageBreak();
			paragraph = document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			sectionHead = paragraph.AddFormattedText("Input Data and Assumptions ", TextFormat.Bold);
			sectionHead.Font.Size = 13;
			paragraph.AddLineBreak();

			paragraph.AddText("The result presented here is based on the following parameters/ assumptions: ");

			//Define a table and attach to a section
			this.table = this.document.LastSection.AddTable();
			this.table.Borders.Color = TableBorder;
			this.table.Borders.Width = 0.25;
			this.table.Rows.LeftIndent = 0;
			this.table.Borders.Left.Width = 0.50;
			this.table.Borders.Right.Width = 0.50;

			paragraph = new Paragraph();
			paragraph.AddFormattedText("Input and Assumpmtion", TextFormat.Bold);
			paragraph.AddLineBreak();

			//Define column
			Column column;
			column = this.table.AddColumn("6cm");
			column = this.table.AddColumn("6cm");
			column = this.table.AddColumn("6cm");

			Row row = this.table.AddRow();
			row.TopPadding = 1.5;
			row.HeadingFormat = true;
			row.Format.Alignment = ParagraphAlignment.Center;
			row.VerticalAlignment = VerticalAlignment.Bottom;
			row.Format.Font.Bold = true;
			row.Cells[0].AddParagraph("Data/Parameter");
			row.Cells[1].AddParagraph("Value/Assumption");
			row.Cells[2].AddParagraph("Source/Remarks");

			row = this.table.AddRow();
			row.Cells[0].AddParagraph("Biomass Price At Farm Gate");
			row.Cells[1].AddParagraph(string.Format("{0} per ton", rvm.BiomassPriceAtFarmGate.GetValueOrDefault().ToString("C0")));
			row.Cells[2].AddParagraph(String.Empty);


			row = this.table.AddRow();
			row.Cells[0].AddParagraph("Project Size");
			row.Cells[1].AddParagraph(string.Format("{0} ", rvm.ProjectSize));
			row.Cells[2].AddParagraph(String.Empty);


			row = this.table.AddRow();
			row.Cells[0].AddParagraph("Land Cost");
			row.Cells[1].AddParagraph(string.Format("{0}", rvm.LandCost));
			row.Cells[2].AddParagraph(String.Empty);

			row = this.table.AddRow();
			row.Cells[0].AddParagraph("Average Cost");
			row.Cells[1].AddParagraph(string.Format("{0}", rvm.AverageCostPerAcre));
			row.Cells[2].AddParagraph(String.Empty);

			row = this.table.AddRow();
			row.Cells[0].AddParagraph("Average Production");
			row.Cells[1].AddParagraph(string.Format("{0} per acre", rvm.AverageProdutivityPerAcre));
			row.Cells[2].AddParagraph(String.Empty);

			paragraph.AddLineBreak();
			paragraph.AddFormattedText("Users can change the inputs to analyze how such change may impact the result",
				TextFormat.Bold);
			paragraph.AddLineBreak();
		}

		private void RenderMoreInformation()
		{
			Paragraph paragraph;
			FormattedText sectionHead;
			Hyperlink h;
			document.LastSection.AddPageBreak();
			paragraph = document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			sectionHead = paragraph.AddFormattedText("More Information ", TextFormat.Bold);
			sectionHead.Font.Size = 13;
			paragraph.AddLineBreak();

			paragraph.AddFormattedText("Additional information about " +
			                           "growing biomass crops may be obtained from county and university extension professionals and USDA " +
			                           "Farm Service Agents in your area. The following contacts may be useful:");
			paragraph.AddLineBreak();

			/* TODO  -Add Relevant Contat */
			paragraph.AddText("N/A");
			paragraph.AddLineBreak();

			paragraph.AddLineBreak();
			paragraph.AddFormattedText("Questions", TextFormat.Bold);
			paragraph.AddLineBreak();

			/* TODO  -Add Questions */
			paragraph.AddLineBreak();

			paragraph.AddText("If you still have questions, you may ");
			h = paragraph.AddHyperlink("/Home/AskExpert", HyperlinkType.Url);
			h.AddFormattedText("Ask an Expert ", TextFormat.Underline);
			paragraph.AddText(".");
			paragraph.AddLineBreak();

			paragraph.AddText("In addition, you may also participate in ");
			h = paragraph.AddHyperlink("/forum", HyperlinkType.Url);
			h.AddFormattedText("the discussion forum", TextFormat.Underline);
			paragraph.AddText(".");
			paragraph.AddLineBreak();
		}

		private void RenderDisclaimer()
		{
			Paragraph paragraph;
			paragraph = this.document.LastSection.AddParagraph();
			paragraph.Format.SpaceBefore = "1cm";
			paragraph.Format.Borders.Width = 0.75;
			paragraph.Format.Borders.Distance = 3;
			paragraph.Format.Borders.Color = TableBorder;
			paragraph.Format.Shading.Color = TableGray;
			paragraph.Format.Font.Color = Color.Parse("red");

			paragraph.AddFormattedText("Disclaimer", TextFormat.Bold);
			paragraph.AddLineBreak();
			paragraph.AddText(
				"The Biomass Decision Support System (BDSS) is provided on an 'as is' basis. " +
				"The University of Tennessee and the developer of BDSS make no warranty that any " +
				"part of BDSS and including the results generated by this website is suitable for " +
				"any particular purpose or is error - free.Use of the BDSS, its result and the " +
				"content of this website at user's sole risk.");
		}

		private void AddHeaderAndFooter()
		{

			var hr = this.document.AddStyle( "HorizontalRule", "Normal" );
			var hrBorder = new Border();
			hrBorder.Width = "1pt";
			hrBorder.Color = Colors.Black;
			hr.ParagraphFormat.Borders.Bottom = hrBorder;
			hr.ParagraphFormat.LineSpacing = 0;
			hr.ParagraphFormat.SpaceBefore = 15;

			Section section = document.LastSection; 
			Paragraph paragraph;
			var header = string.Format("{0} in {1}, {2}", rvm.CropType, rvm.CountyName, rvm.StateName);
			paragraph = new Paragraph();
			paragraph.Style = "HorizontalRule"; 
			paragraph.AddTab();
			paragraph.AddText(header);
			paragraph.Format.Font.Size = 9;
			paragraph.Format.Alignment = ParagraphAlignment.Right;
			section.Headers.Primary.Add(paragraph);
			section.Headers.EvenPage.Add(paragraph.Clone());

			var hrt = this.document.AddStyle( "HorizontalRuleTop", "Normal" );
			hrBorder = new Border();
			hrBorder.Width = "1pt";
			hrBorder.Color = Colors.Black;
			hrt.ParagraphFormat.Borders.Top = hrBorder;
			hrt.ParagraphFormat.LineSpacing = 0;
			hrt.ParagraphFormat.SpaceAfter = 5;

			paragraph = new Paragraph();
			paragraph.Style = "HorizontalRuleTop";

			paragraph.AddTab();
			paragraph.AddText("Page ");
			paragraph.AddPageField();
			paragraph.AddText( " of " );
			paragraph.AddNumPagesField();
			paragraph.Format.Font.Size = 9;
			paragraph.Format.Alignment = ParagraphAlignment.Center;
			section.Footers.Primary.Add(paragraph);
			section.Footers.EvenPage.Add(paragraph.Clone());
		}

		private void GetImage( string keyValue, string imageCaption = null )
		{
			string img;
			Paragraph paragraph;
			img = GetChart( keyValue );
			if ( img != null )
			{
				paragraph = document.LastSection.AddParagraph();
				paragraph.AddLineBreak();
				paragraph.Format.Alignment = ParagraphAlignment.Center;
				var map = paragraph.AddImage( img );
				map.Width = Unit.FromCentimeter( 12 );
				paragraph.AddLineBreak();
				if ( imageCaption != null )
				{
					paragraph.AddFormattedText( imageCaption);
					paragraph.AddLineBreak();
				}

			}
		}

		private static string GetChart( string keyValue )
		{

			var chart = Chart.GetFromCache( keyValue );
			var bytes = chart.GetBytes();
			var img = FileFromBytes( bytes );
			return img;

		}
		private static string FileFromBytes( byte[] image )
		{
			if ( image == null )
				return null;
			return "base64:" +
			Convert.ToBase64String( image );
		}

		public async Task<byte[]> GetStaticMap( string location = "center=43.07,-89.40", string size = "400x400", string maptype = "roadmap" )
		{
			if ( string.IsNullOrEmpty( location ) )
			{
				location = "center=43.07,-89.40";
			}

			if ( string.IsNullOrEmpty( size ) )
			{
				size = "400x300";
			}

			if (
				string.IsNullOrEmpty( maptype )
				|| ( !maptype.Equals( "roadmap", StringComparison.InvariantCultureIgnoreCase ) )
				&& ( !maptype.Equals( "hybrid", StringComparison.InvariantCultureIgnoreCase ) )
				&& ( !maptype.Equals( "satellite", StringComparison.InvariantCultureIgnoreCase ) )
				&& ( !maptype.Equals( "terrain", StringComparison.InvariantCultureIgnoreCase ) ) )
			{
				maptype = "road";
			}
			try
			{
				//var stasticMapApiUrl = "http://maps.googleapis.com/maps/api/staticmap?&markers=color:navy%7Clabel:R%7C62.107733," + location + "&zoom=12&maptype=hybrid&sensor=false";
				var stasticMapApiUrl = "http://maps.googleapis.com/maps/api/staticmap?" + location + "&zoom=9&maptype=" + maptype + "&sensor=false";
				var formattedImageUrl = string.Format( "{0}&size={1}", string.Format( "{0}&center={1}", stasticMapApiUrl, location ), size );
				var httpClient = new HttpClient();

				var imageTask = httpClient.GetAsync( formattedImageUrl );
				HttpResponseMessage response = imageTask.Result;
				response.EnsureSuccessStatusCode();

				await response.Content.LoadIntoBufferAsync();
				var data = await response.Content.ReadAsByteArrayAsync();

				return data;
			}
			catch ( Exception exception )
			{
				return null;
			}
		}

		/// <summary>
		/// Creates the dynamic parts of the invoice.
		/// </summary>
		void FillContent()
		{
			// Fill address in address text frame

			Paragraph paragraph = this.addressFrame.AddParagraph();
			paragraph.AddText( "Dr. Anwar Ali" );
			paragraph.AddLineBreak();
			paragraph.AddText( "Health And Social Services " );
			paragraph.AddLineBreak();
			paragraph.AddText( "Karachi" );

			Row row1;
			for ( int i = 0; i < dt.Rows.Count; i++ )
			{
				row1 = this.table.AddRow();


				row1.TopPadding = 1.5;


				for ( int j = 0; j < dt.Columns.Count; j++ )
				{
					row1.Cells[j].Shading.Color = TableGray;
					row1.Cells[j].VerticalAlignment = VerticalAlignment.Center;
					row1.Cells[j].Format.Alignment = ParagraphAlignment.Left;
					row1.Cells[j].Format.FirstLineIndent = 1;
					row1.Cells[j].AddParagraph( dt.Rows[i][j].ToString() );
					this.table.SetEdge( 0, this.table.Rows.Count - 2, dt.Columns.Count, 1, Edge.Box, BorderStyle.Single, 0.75 );
				}
			}


			// Add the notes paragraph
			paragraph = this.document.LastSection.AddParagraph();
			paragraph.Format.SpaceBefore = "1cm";
			paragraph.Format.Borders.Width = 0.75;
			paragraph.Format.Borders.Distance = 3;
			paragraph.Format.Borders.Color = TableBorder;
			paragraph.Format.Shading.Color = TableGray;
			paragraph.AddText( "Note: For any complain please contact us in 24 hours from the issuance of bill" );


		}

		// Some pre-defined colors
#if true
		// RGB colors
		readonly static Color TableBorder = new Color( 81, 125, 192 );
		readonly static Color TableBlue = new Color( 235, 240, 249 );
		readonly static Color TableGray = new Color( 242, 242, 242 );
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



/*  Table based approach on test */

/*
//Define a table and attach to a section
this.table = this.document.LastSection.AddTable();
this.table.Borders.Color = Colors.Transparent;
this.table.Borders.Width = 0.25;
this.table.Rows.LeftIndent = 0;
this.table.Borders.Left.Width = 0.50;
this.table.Borders.Right.Width = 0.50;

Paragraph paragraphInput = new Paragraph();
paragraph.AddFormattedText("Input and Assumpmtion", TextFormat.Bold);

//Define column
Column column;
column = this.table.AddColumn( "9cm" );
column = this.table.AddColumn( "8cm" );

Row row = this.table.AddRow();
row.TopPadding = 1.5;
row.Cells[0].AddParagraph( summary );

var p =  row.Cells[1].AddParagraph();
p.AddFormattedText("Input and Assumpmtion", TextFormat.Bold);
p.AddLineBreak();
p.AddText("Map goes here");

*/
