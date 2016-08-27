using System;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Compilation;
using System.Web.Helpers;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;
using BiofuelSouth.ViewModels;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
// using System.Xml.XPath;

namespace BiofuelSouth.Manager
{
	// ReSharper disable once InconsistentNaming
	public class PDFform
	{
		ResultViewModel Rvm { get; }
		public PDFform( ResultViewModel rv )
		{
			Rvm = rv;
			GetTable();
		}

		readonly string _baseUrl = ConfigurationManager.AppSettings["baseUrl"] ?? "http://biofuelsouth.azurewebsites.net/";
		readonly string _farmserviceContactUrl = ConfigurationManager.AppSettings["farmserviceContactUrl"] ??
			"http://www.fsa.usda.gov/state-offices/index";
		
		Document _document;

/*
		TextFrame _addressFrame;
*/

		/// <summary>
		/// The table of the MigraDoc document that contains the invoice items.
		/// </summary>
		Table _table;


		/// <summary>
		/// This example method generates a DataTable.
		/// </summary>
		private void GetTable()
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
		}

		/// <summary>
		/// Creates the invoice document.
		/// </summary>
		public Document CreateDocument()
		{
			// Create a new MigraDoc document
			_document = new Document();
			_document.DefaultPageSetup.PageFormat = PageFormat.Letter;

			_document.Info.Title = "DSS Results";
			_document.Info.Subject = "Report";
			_document.Info.Author = "Biomass Decision Support System";

			DefineStyles();
			CreateResultSummaryPage();
			AddHeaderAndFooter();



			return _document;
		}

		/// <summary>
		/// Defines the styles used to format the MigraDoc document.
		/// </summary>
		void DefineStyles()
		{
			// Get the predefined style Normal.
			Style style = _document.Styles["Normal"];
			// Because all styles are derived from Normal, the next line changes the 
			// font of the whole document. Or, more exactly, it changes the font of
			// all styles and paragraphs that do not redefine the font.
			style.Font.Name = "Verdana";

			style = _document.Styles[StyleNames.Header];
			style.ParagraphFormat.AddTabStop( "16cm", TabAlignment.Right );

			style = _document.Styles[StyleNames.Footer];
			style.ParagraphFormat.AddTabStop( "8cm", TabAlignment.Center );

			// Create a new style called Table based on style Normal
			style = _document.Styles.AddStyle( "Table", "Normal" );
			style.Font.Name = "Verdana";
			style.Font.Name = "Times New Roman";
			style.Font.Size = 9;

			// Create a new style called Reference based on style Normal
			style = _document.Styles.AddStyle( "Reference", "Normal" );
			style.ParagraphFormat.SpaceBefore = "5mm";
			style.ParagraphFormat.SpaceAfter = "5mm";
			style.ParagraphFormat.TabStops.AddTabStop( "16cm", TabAlignment.Right );
		}


		void CreateResultSummaryPage()
		{
			// Each MigraDoc document needs at least one section.
			var section = _document.AddSection();


			// Add the print date field
			var paragraph = section.AddParagraph();
			paragraph.Format.SpaceBefore = "0cm";
			paragraph.Style = "Reference";


			var cropImage = paragraph.AddImage( HttpContext.Current.Server.MapPath( "~/images/ibssparnershiplogo.png" ) );
			cropImage.Width = Unit.FromCentimeter( 6);
			cropImage.LockAspectRatio = true;
			paragraph.AddLineBreak();


			paragraph.AddFormattedText( "Biomass Decision Support System" );
			paragraph.AddLineBreak();

			var cropMsg = $"Results for {Rvm.CropType} in {Rvm.CountyName}, {Rvm.StateName}";
			paragraph.AddFormattedText( cropMsg, TextFormat.Bold );
			paragraph.AddLineBreak();
			paragraph.AddText( "Date: " );
			paragraph.AddDateField( "MM/dd/yyyy" );
			paragraph.Format.Alignment = ParagraphAlignment.Center;


			#region Summary 
			paragraph = _document.LastSection.AddParagraph();
			paragraph.Style = "Reference";
			var sectionHead = paragraph.AddFormattedText( "Summary", TextFormat.Bold );
			sectionHead.Font.Size = 13;

			paragraph.AddLineBreak();

			//paragraph.AddText( summary );

			paragraph.AddText( "Growing of " );
			paragraph.AddFormattedText( Rvm.CropType.ToString(), TextFormat.Bold );

			paragraph.AddText( " for a duration of " );
			paragraph.AddFormattedText( Rvm.ProjectLife.ToString(), TextFormat.Bold );


			paragraph.AddText( " year(s) over an area of " );
			paragraph.AddFormattedText( Rvm.ProjectSize, TextFormat.Bold );

			paragraph.AddText( " in " );
			paragraph.AddFormattedText( Rvm.CountyName, TextFormat.Bold );

			paragraph.AddFormattedText( " County, ", TextFormat.Bold );
			paragraph.AddFormattedText( Rvm.StateName, TextFormat.Bold );

			paragraph.AddText( " is expected to produce an estimated " );
			paragraph.AddFormattedText( Rvm.AnnualProduction, TextFormat.Bold );

			paragraph.AddText( " tons of biomass annually." );

			paragraph = _document.LastSection.AddParagraph();
			paragraph.Style = "Reference";
			paragraph.Format.Alignment = ParagraphAlignment.Center;
			cropImage = paragraph.AddImage( HttpContext.Current.Server.MapPath( Rvm.ImageUrl.Item3 ) );
			cropImage.Width = Unit.FromCentimeter( 8 );
			cropImage.LockAspectRatio = true;
			paragraph.AddLineBreak();
			paragraph.AddFormattedText( Rvm.ImageUrl.Item2 );

			var lat = Rvm.CountyEntity.Lat;
			var lon = Rvm.CountyEntity.Lon;
			var location = $"center={lat},{lon}";
			Task<byte[]> result = GetStaticMap( location, "400x280" );

			{
				paragraph = _document.LastSection.AddParagraph();
				paragraph.AddLineBreak();
				paragraph.Format.Alignment = ParagraphAlignment.Center;
				var file = result.Result;
				string imgfile = FileFromBytes( file );

				paragraph.AddLineBreak();
				var map = paragraph.AddImage( imgfile );
				map.Height = Unit.FromCentimeter( 5 );
				paragraph.AddLineBreak();
				paragraph.AddFormattedText( "Approximate Location of Assessment Area" );
				paragraph.AddLineBreak();
			}


			paragraph = section.AddParagraph();
			paragraph.Style = "Reference";
			paragraph.AddText( "The production comes at an expected annual cost of " );
			paragraph.AddFormattedText( Rvm.AnnualCost, TextFormat.Bold );
			paragraph.AddText( " and results in an annual revenue of " );
			paragraph.AddFormattedText( Rvm.AnnualRevenue, TextFormat.Bold );
			paragraph.AddText( ". " );

			paragraph.AddText( "The " );
			var h = paragraph.AddHyperlink( $"{_baseUrl}Home/Search?term=NPV", HyperlinkType.Url );
			h.AddFormattedText( "net present value (NPV) ", TextFormat.Underline );
			paragraph.AddText( "of the project is estimated to be " );
			paragraph.AddFormattedText( Rvm.NPV.ToString( "C0" ), TextFormat.Bold );
			paragraph.AddText( " at assumed prevailing interest rate of " );
			paragraph.AddFormattedText( Rvm.InterestRate.ToString( "P" ), TextFormat.Bold );

			if ( Rvm.NPV < 0 )
			{
				paragraph.AddText( "This means there will be a net loss of " );
				var formatted = paragraph.AddFormattedText( Rvm.NPV.ToString( "C0" ), TextFormat.Bold );
				formatted.Color = Colors.Red;
			}
			else
			{
				paragraph.AddText( "This means there will be a net profit of " );
				paragraph.AddFormattedText( Rvm.NPV.ToString( "C0" ), TextFormat.Bold );
			}
			paragraph.AddText( " from this project during project's life (" );
			paragraph.AddFormattedText( Rvm.ProjectLife.ToString(), TextFormat.Bold );
			paragraph.AddText( " years)." );


			paragraph.AddLineBreak();



			//comparision with other crops
			paragraph = _document.LastSection.AddParagraph();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			paragraph.AddLineBreak();
			paragraph.AddFormattedText( "Comparison with other crops", TextFormat.Bold );
			paragraph.AddLineBreak();

			// ReSharper disable once InconsistentNaming
			var highNPV = Rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.HighNpv );
			// ReSharper disable once InconsistentNaming
			var lowNPV = Rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.LowNpv );

			if ( highNPV != null && highNPV.Crop != Rvm.CropType )
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
			else if ( highNPV != null && highNPV.Crop == Rvm.CropType )
			{
				paragraph.AddText( "It is anticipated that growing " );
				paragraph.AddFormattedText( highNPV.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may likely result in highest profit compared to other crops." );

			}
			if ( lowNPV != null && lowNPV.Crop != Rvm.CropType )
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
			else if ( lowNPV != null && lowNPV.Crop == Rvm.CropType )
			{
				//paragraph.AddLineBreak();
				paragraph.AddText( "Likewise, it is anticipated that growing " );
				paragraph.AddFormattedText( lowNPV.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may result the lowest profit compared to growing other crops." );
			}
			string keyValue;
			if ( Rvm.ChartKeys.Select( m => m.Key == ChartType.NPVCompare ).Any() )
			{
				keyValue = Rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.NPVCompare ).Value;
				GetImage( keyValue, "Comparision of project NPV ($) across various biofuel crops" );
			}


			#endregion Summary
			//Production Results:
			paragraph = _document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			sectionHead = paragraph.AddFormattedText( "Production Results", TextFormat.Bold );
			sectionHead.Font.Size = 13;

			paragraph.AddLineBreak();
			paragraph.AddText( "Growing of " );
			paragraph.AddFormattedText( Rvm.CropType.ToString(), TextFormat.Bold );
			paragraph.AddText( " for a duration of " );
			paragraph.AddFormattedText( Rvm.ProjectLife.ToString(), TextFormat.Bold );
			paragraph.AddText( "  years over an area of " );
			paragraph.AddFormattedText( Rvm.ProjectSize, TextFormat.Bold );
			paragraph.AddText( "  in " );

			paragraph.AddFormattedText( Rvm.CountyName, TextFormat.Bold );

			paragraph.AddFormattedText( " County, ", TextFormat.Bold );
			paragraph.AddFormattedText( Rvm.StateName, TextFormat.Bold );

			paragraph.AddText( " is expected to produce an estimated " );
			paragraph.AddFormattedText( Rvm.AnnualProduction, TextFormat.Bold );

			paragraph.AddText( " tons of biomass annually." );


			if ( Rvm.ChartKeys.Select( m => m.Key == ChartType.Production ).Any() )
			{
				keyValue = Rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.Production ).Value;
				GetImage( keyValue, Constants.ProductionChartCaption( Rvm.CropType ) );
			}

			#region production comparision
			paragraph = _document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			paragraph.AddFormattedText( "Comparison with other crops", TextFormat.Bold );
			var high = Rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.HighProduction );
			var low = Rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.LowProduction );

			if ( high != null )
			{
				paragraph.AddLineBreak();
				paragraph.AddText( "It is anticipated that growing " );
				paragraph.AddFormattedText( high.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may likely result in the highest production of " );
				paragraph.AddFormattedText( high.ComparisionValue, TextFormat.Bold );

				if ( high.Crop != Rvm.CropType )
				{

					paragraph.AddText( " tons." );
				}

				else if ( high.Crop == Rvm.CropType )
				{
					paragraph.AddText( " tons compared to other crops." );
				}
			}


			if ( low != null )
			{
				paragraph.AddText( "It is anticipated that growing " );
				paragraph.AddFormattedText( low.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may likely result in the highest production of " );
				paragraph.AddFormattedText( low.ComparisionValue, TextFormat.Bold );
				if ( low.Crop != Rvm.CropType )
				{
					paragraph.AddText( " tons." );
				}
				else if ( low.Crop == Rvm.CropType )
				{
					paragraph.AddText( " tons compared to other crops." );
				}
			}

			if ( Rvm.ChartKeys.Select( m => m.Key == ChartType.ProductionCompare ).Any() )
			{
				keyValue = Rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.ProductionCompare ).Value;
				GetImage( keyValue, Constants.ProductionCompareChartCaption( Rvm.CropType ) );
			}
			#endregion production comparision

			#region Cost and revenue
			paragraph = _document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			sectionHead = paragraph.AddFormattedText( "Cost And Revenue", TextFormat.Bold );
			sectionHead.Font.Size = 13;
			paragraph.AddLineBreak();

			paragraph.AddText( "The production comes at an expected annual cost of " );
			paragraph.AddFormattedText( Rvm.AnnualCost, TextFormat.Bold );

			paragraph.AddText( " and results in an annual revenue of " );
			paragraph.AddFormattedText( Rvm.AnnualRevenue, TextFormat.Bold );

			paragraph.AddText( ". The net present value of the project is estimated to be " );
			paragraph.AddFormattedText( Rvm.NPV.ToString( "C0" ), TextFormat.Bold );

			paragraph.AddText( "at assumed prevailing interest rate of " );
			paragraph.AddFormattedText( Rvm.InterestRate.ToString( "P" ), TextFormat.Bold );
			paragraph.AddText( "." );

			if ( Rvm.ChartKeys.Select( m => m.Key == ChartType.CostRevenue ).Any() )
			{
				keyValue = Rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.CostRevenue ).Value;
				GetImage( keyValue, "Cost and revenue information" );
			}

			paragraph = _document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.AddFormattedText( "Comparison with other crops", TextFormat.Bold );
			paragraph.AddLineBreak();

			var highPrice = Rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.HighFarmGatePrice );
			var lowPrice = Rvm.ComparisionData.FirstOrDefault( m => m.Key == ResultComparisionKey.LowFarmgatePrice );

			if ( highPrice != null )
			{
				paragraph.AddText( "Among the biomass crops considered in this particular analysis, " +
								   "it is anticipated that growing " );
				paragraph.AddFormattedText( highPrice.Crop.ToString(), TextFormat.Bold );

				if ( highPrice.Crop != Rvm.CropType )
				{
					paragraph.AddText( " may likely result in the highest farmgate price of " );
					paragraph.AddFormattedText( highPrice.ComparisionValue, TextFormat.Bold );
					paragraph.AddText( " per ton." );
				}
				else if ( highPrice.Crop == Rvm.CropType )
				{
					paragraph.AddText( " is the option that results in the highest farmgate price of " );
					paragraph.AddFormattedText( highPrice.ComparisionValue, TextFormat.Bold );
					paragraph.AddText( " per ton compared to other crops." );
				}
			}

			if ( lowPrice != null )
			{
				paragraph.AddText( " Likewise, it is anticipated that growing " );
				paragraph.AddFormattedText( lowPrice.Crop.ToString(), TextFormat.Bold );
				paragraph.AddText( " may likely result in the lowest farmgate price of " );
				paragraph.AddFormattedText( lowPrice.ComparisionValue, TextFormat.Bold );
				
				if ( lowPrice.Crop != Rvm.CropType )
				{
					paragraph.AddText( " per ton." );
				}
				else if ( lowPrice.Crop == Rvm.CropType )
				{
					paragraph.AddText( " per ton compared to growing other crops." );
				}
				paragraph.AddLineBreak();
			}

			if ( Rvm.ChartKeys.Select( m => m.Key == ChartType.CashFlow ).Any() )
			{
				keyValue = Rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.CashFlow ).Value;
				GetImage( keyValue, Constants.CashFlowChartCaption( Rvm.CropType ) );

			}

			if ( Rvm.ChartKeys.Select( m => m.Key == ChartType.CashFlowCompare ).Any() )
			{
				keyValue = Rvm.ChartKeys.FirstOrDefault( m => m.Key == ChartType.CashFlowCompare ).Value;

				GetImage( keyValue, Constants.CashFlowCompareChartCaption( Rvm.CropType ) );
			}

			#endregion
			RenderInputAndAssumption();
			RenderMoreInformation();
			RenderDisclaimer();
		}

		private void RenderInputAndAssumption()
		{
			_document.LastSection.AddPageBreak();
			var paragraph = _document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			var sectionHead = paragraph.AddFormattedText( "Input Data and Assumptions ", TextFormat.Bold );
			sectionHead.Font.Size = 13;
			paragraph.AddLineBreak();

			paragraph.AddText( "The result presented here is based on the following parameters/ assumptions: " );

			//Define a table and attach to a section
			_table = _document.LastSection.AddTable();
			_table.Borders.Color = TableBorder;
			_table.Borders.Width = 0.25;
			_table.Rows.LeftIndent = 0;
			_table.Borders.Left.Width = 0.50;
			_table.Borders.Right.Width = 0.50;

			paragraph = new Paragraph();
			paragraph.AddFormattedText( "Input and Assumpmtion", TextFormat.Bold );
			paragraph.AddLineBreak();

			//Define column
			_table.AddColumn( "6cm" );
			_table.AddColumn( "6cm" );
			_table.AddColumn( "6cm" );

			Row row = _table.AddRow();
			row.TopPadding = 1.5;
			row.HeadingFormat = true;
			row.Format.Alignment = ParagraphAlignment.Center;
			row.VerticalAlignment = VerticalAlignment.Bottom;
			row.Format.Font.Bold = true;
			row.Cells[0].AddParagraph( "Data/Parameter" );
			row.Cells[1].AddParagraph( "Value/Assumption" );
			row.Cells[2].AddParagraph( "Source/Remarks" );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "County, State" );
			row.Cells[1].AddParagraph(Rvm.CountyName + " county, " + Rvm.StateName + " ");
			row.Cells[2].AddParagraph( String.Empty );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Project Duration" );
			row.Cells[1].AddParagraph( $"{Rvm.ProjectLife} years" );
			row.Cells[2].AddParagraph( String.Empty );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Project Size" );
			row.Cells[1].AddParagraph( $"{Rvm.ProjectSize} " );
			row.Cells[2].AddParagraph( String.Empty );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Interest Rate" );
			row.Cells[1].AddParagraph( $"{Rvm.InterestRate.ToString("P")} " );
			row.Cells[2].AddParagraph( String.Empty );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Biomass Price At Farm Gate" );
			row.Cells[1].AddParagraph($"{Rvm.BiomassPriceAtFarmGate.GetValueOrDefault().ToString("C0")} per ton");
			row.Cells[2].AddParagraph( String.Empty );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Land Cost" );
			row.Cells[1].AddParagraph( $"{Rvm.LandCost}" );
			row.Cells[2].AddParagraph( String.Empty );


			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Average Cost" );
			row.Cells[1].AddParagraph( $"{Rvm.AverageCostPerAcre}" );
			row.Cells[2].AddParagraph( String.Empty );

			row = _table.AddRow();
			row.Cells[0].AddParagraph( "Average Production" );
			row.Cells[1].AddParagraph( $"{Rvm.AverageProdutivityPerAcre} per acre" );
			row.Cells[2].AddParagraph( String.Empty );





			if ( !Rvm.RequireStorage)
			{
				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Crop Storage" );
				row.Cells[1].AddParagraph( "Not Required/Not Applicable" );
				row.Cells[2].AddParagraph( String.Empty );

			}
			else
			{
				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Storage Time" );
				row.Cells[1].AddParagraph( Rvm.StorageTime.ToString(CultureInfo.InvariantCulture) + " (days)");
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Storage Percent" );
				row.Cells[1].AddParagraph( Rvm.StoragePercent.ToString( "P"));
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Storage Method" );
				row.Cells[1].AddParagraph( Rvm.StorageMethod);
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Storage Cost Assessment Method" );
				row.Cells[1].AddParagraph( Rvm.StorageCostmethod);
				row.Cells[2].AddParagraph( String.Empty );

			}


			if ( !Rvm.RequireFinance )
			{
				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Financing" );
				row.Cells[1].AddParagraph( "Not Required/Not Applicable" );
				row.Cells[2].AddParagraph( String.Empty );

			}
			else
			{
				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Administrative Cost" );
				row.Cells[1].AddParagraph( Rvm.AdministrativeCost.ToString( "C0" ) + "/acre/year" );
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Incentive Payment" );
				row.Cells[1].AddParagraph( Rvm.IncentivePayment.ToString( "C0" ) + "/acre/year" );
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Number of years of incentive payment" );
				row.Cells[1].AddParagraph( Rvm.IncentivePayment.ToString("##,###")  + " years" );
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Available Equity" );
				row.Cells[1].AddParagraph( Rvm.AvailableEquity.ToString("C0") );
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Loan Amount" );
				row.Cells[1].AddParagraph( (Rvm.LoanAmount/100).ToString( "C0" ) );
				row.Cells[2].AddParagraph( String.Empty );

				row = _table.AddRow();
				row.Cells[0].AddParagraph( "Equity Loan Interest Rate" );
				row.Cells[1].AddParagraph( Rvm.EquityLoanInterestRate.ToString( "P" ) );
				row.Cells[2].AddParagraph( String.Empty );

			}


			paragraph.AddLineBreak();
			paragraph.AddFormattedText( "Users can change the inputs to analyze how such change may impact the result",
				TextFormat.Bold );
			paragraph.AddLineBreak();
		}

		private void RenderMoreInformation()
		{
			_document.LastSection.AddPageBreak();
			var paragraph = _document.LastSection.AddParagraph();
			paragraph.AddLineBreak();
			paragraph.Format.Alignment = ParagraphAlignment.Left;
			var sectionHead = paragraph.AddFormattedText( "More Information ", TextFormat.Bold );
			sectionHead.Font.Size = 13;
			paragraph.AddLineBreak();

			paragraph.AddFormattedText("Additional information about " +
			                           "growing biomass crops may be obtained from county and " +
									   "university extension professionals and USDA Farm Service Agents " +
			                           "in your by visiting " );
									   
			paragraph.AddLineBreak();

			
			
			var h = paragraph.AddHyperlink( $" {_farmserviceContactUrl}", HyperlinkType.Url );
			h.AddFormattedText( $"{_farmserviceContactUrl}", TextFormat.Underline );
			h.AddText(".");
				

			paragraph.AddLineBreak();

			paragraph.AddLineBreak();
			paragraph.AddFormattedText( "Questions", TextFormat.Bold );
			paragraph.AddLineBreak();

			/* TODO  -Add Questions */
			paragraph.AddLineBreak();

			paragraph.AddText( "If you still have questions, you may " );
			h = paragraph.AddHyperlink($"{_baseUrl}Home/AskExpert", HyperlinkType.Url );
			h.AddFormattedText($"Ask an Expert ({_baseUrl}Home/AskExpert)", TextFormat.Underline );
			paragraph.AddText( "." );
			paragraph.AddLineBreak();
			paragraph.AddLineBreak();

			paragraph.AddText( "In addition, you may also participate in " );
			h = paragraph.AddHyperlink( $"{_baseUrl}forum", HyperlinkType.Url );
			h.AddFormattedText( $"the discussion forum ({_baseUrl}forum)", TextFormat.Underline );
			paragraph.AddText( "." );
			paragraph.AddLineBreak();
		}

		private void RenderDisclaimer()
		{
			var paragraph = _document.LastSection.AddParagraph();
			paragraph.Format.SpaceBefore = "1cm";
			paragraph.Format.Borders.Width = 0.75;
			paragraph.Format.Borders.Distance = 3;
			paragraph.Format.Borders.Color = TableBorder;
			paragraph.Format.Shading.Color = TableGray;
			paragraph.Format.Font.Color = Color.Parse( "red" );

			paragraph.AddFormattedText( "Disclaimer", TextFormat.Bold );
			paragraph.AddLineBreak();
			paragraph.AddText( @"The Biomass Decision Support System (BDSS) is provided on an ""as is"" basis. While steps have been taken to ensure that the input data behind it, the algorithms which operate within it and its overall functionality are accurate and reliable, the University of Tennessee and the developer of BDSS make no warranty that any part of BDSS and including the results generated by this website is suitable for any particular purpose or is error-free. IBSS along with its partner institutions and funding agency, and the developer of BDSS give no warranty and make no representation as to its accuracy and accept no liability in any way whatsoever for any omissions or errors contained within it nor for any losses incurred (either direct or indirect) as a result of its use. Use of the BDSS, its result and the content of this website at user's sole risk and it is the responsibility of individual or organization using this tool to satisfy themselves as to the validity of any outputs derived from BDSS." );


		}

		private void AddHeaderAndFooter()
		{

			var hr = _document.AddStyle( "HorizontalRule", "Normal" );
			var hrBorder = new Border
			{
				Width = "1pt",
				Color = Colors.Black
			};
			hr.ParagraphFormat.Borders.Bottom = hrBorder;
			hr.ParagraphFormat.LineSpacing = 0;
			hr.ParagraphFormat.SpaceBefore = 15;

			

			Section section = _document.LastSection;
			var header = $"{Rvm.CropType} in {Rvm.CountyName}, {Rvm.StateName}";
			var paragraph = new Paragraph {Style = "HorizontalRule"};


			paragraph.AddTab();
			paragraph.AddText( header );
			paragraph.Format.Font.Size = 9;
			paragraph.Format.Alignment = ParagraphAlignment.Right;
			section.Headers.Primary.Add( paragraph );
			section.Headers.EvenPage.Add( paragraph.Clone() );

			var hrt = _document.AddStyle( "HorizontalRuleTop", "Normal" );
			hrBorder = new Border
			{
				Width = "1pt",
				Color = Colors.Black
			};
			hrt.ParagraphFormat.Borders.Top = hrBorder;
			hrt.ParagraphFormat.LineSpacing = 0;
			hrt.ParagraphFormat.SpaceAfter = 9;

			paragraph = new Paragraph {Style = "HorizontalRuleTop"};

			paragraph.AddTab();
			paragraph.AddText( "Page " );
			paragraph.AddPageField();
			paragraph.AddText( " of " );
			paragraph.AddNumPagesField();
			paragraph.Format.Font.Size = 9;
			paragraph.Format.Alignment = ParagraphAlignment.Center;
			section.Footers.Primary.Add( paragraph );
			section.Footers.EvenPage.Add( paragraph.Clone() );
		}

		private void GetImage( string keyValue, string imageCaption = null )
		{
			var img = GetChart( keyValue );
			if ( img != null )
			{
				var paragraph = _document.LastSection.AddParagraph();
				paragraph.AddLineBreak();
				paragraph.Format.Alignment = ParagraphAlignment.Center;
				var map = paragraph.AddImage( img );
				map.Width = Unit.FromCentimeter( 12 );
				paragraph.AddLineBreak();
				if ( imageCaption != null )
				{
					paragraph.AddFormattedText( imageCaption );
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
				var formattedImageUrl =
					$"{string.Format($"{stasticMapApiUrl}&center={location}", stasticMapApiUrl, location)}&size={size}";
				var httpClient = new HttpClient();

				var imageTask = httpClient.GetAsync( formattedImageUrl );
				HttpResponseMessage response = imageTask.Result;
				response.EnsureSuccessStatusCode();

				await response.Content.LoadIntoBufferAsync();
				var data = await response.Content.ReadAsByteArrayAsync();

				return data;
			}
			catch (Exception)
			{
				return null;
			}
		}

		// Some pre-defined colors
#if true
		// RGB colors
		static readonly Color TableBorder = new Color( 81, 125, 192 );
		static readonly Color TableGray = new Color( 242, 242, 242 );
#else
    // CMYK colors
    readonly static Color tableBorder = Color.FromCmyk(100, 50, 0, 30);
    readonly static Color tableBlue = Color.FromCmyk(0, 80, 50, 30);
    readonly static Color tableGray = Color.FromCmyk(30, 0, 0, 0, 100);
#endif
	}
}
