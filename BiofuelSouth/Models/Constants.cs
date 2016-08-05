using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using BiofuelSouth.Enum;
using BiofuelSouth.Models.Entity;
using BiofuelSouth.Services;

namespace BiofuelSouth.Models
{
	public static partial class Constants
	{

		// Weigth of bale for miscanthus was same as for the weight of switchgrass
		//http://miscanthus.illinois.edu/symposium/2009/jan_13/10_Ulrich.pdf
		private const decimal WeightBaleRound = 650;
		private const decimal WeightBaleRect = 2000;
		private const int BalePerStack = 6;


		private const decimal SqFtPerBaleRound = 36.0m;
		private const decimal SqFtPerBaleRect = 60.0m;

		private const decimal TarpSqFtPerStackRound = 108;
		private const decimal GravelSqFtPerStackRound = 108;
		private const decimal PalletSqFtPerStackRound = 108;

		private const decimal TarpSqFtPerStackRect = 180;
		private const decimal GravelSqFtPerStackRect = 180;
		private const decimal PalletSqFtPerStackRect = 180;

		public static int ProjectLife => 10;


		public static IEnumerable<SelectListItem> GetCategory()
		{
			IList<SelectListItem> items = new List<SelectListItem>
			{
				new SelectListItem{Text = @"Switchgrass", Value = "Switchgrass"},
				new SelectListItem{Text = @"Miscanthus", Value = "Miscanthus"},
				new SelectListItem{Text = @"Poplar", Value = "Poplar"},
				new SelectListItem{Text = @"Willow", Value = "Willow"},
				new SelectListItem {Text = @"Pine", Value = "Pine" }
			};
			return items;
		}





		public static IEnumerable<string> GetCounty( String state )
		{
			//TODO could change this to make database driven call.

			var county = DataService.GetCounty( state );
			county.Insert( 0, "Select County" );
			return county;

		}

		public static String CountyName( String geoId )
		{
			using ( var db = new DatabaseContext() )
			{
				var county = db.County.Where( c => c.GeoId == geoId ).Select( a => a.Name ).FirstOrDefault();
				return county;
			}

		}

		public static double GetValue()
		{
			const int intGeoid = 37163;
			const string category = "Switchgrass";
			using ( var db = new DatabaseContext() )
			{

				var productivity = db.Productivities.Where( p => p.GeoId == intGeoid && p.CropType.Equals( category ) ).Select( p => p.Yield ).FirstOrDefault();
				return productivity;
			}

		}

		//Method to return an average price of farm gate price for a crop type
		public static decimal GetFarmGatePrice( CropType cropType )
		{
			switch ( cropType )
			{
				case CropType.Switchgrass:
					return 65.0M; // http://www.uky.edu/Ag/CCD/introsheets/switchgrass.pdf
				case CropType.Miscanthus:
					return 45.0M; //http://pubs.cas.psu.edu/FreePubs/PDFs/ee0081.pdf
				case CropType.Poplar:
					return 30.0M;
				case CropType.Willow:
					return 30.0M;
				case CropType.Pine:
					return 30.0M;
				default:
					return -999.99M;

			}

		}

		public static IList<double> GetProductivityTaper( String cropType )
		{
			switch ( cropType )
			{
				case "Switchgrass":
					return new List<double> { 0.25, 0.5, 1 };

				default:
					return new List<double> { 1 };
			}
		}

		public static IEnumerable<SelectListItem> GetCountySelectList( String state = null )
		{
			IList<CountyEntity> countyList = DataService.GetCountyData( state ?? "ALL" );

			return countyList.Select( c => new SelectListItem { Text = c.Name, Value = c.GeoId } );
		}


		public static IEnumerable<SelectListItem> GetProvincesList()
		{
			IList<SelectListItem> items = new List<SelectListItem>
			{
				new SelectListItem{Text = @"California", Value = "B"},
				new SelectListItem{Text = @"Alaska", Value = "B"},
				new SelectListItem{Text = @"Illinois", Value = "B"},
				new SelectListItem{Text = @"Texas", Value = "B"},
				new SelectListItem{Text = @"Washington", Value = "B"}

			};
			return items;
		}

		public static IEnumerable<SelectListItem> GetYesNo()
		{
			IList<SelectListItem> items = new List<SelectListItem>
			{
				new SelectListItem {Text = @"Yes", Value = "true"},
				new SelectListItem {Text = @"No", Value = "false"}
			 };
			return items;
		}

		/// <summary>
		/// Returns percent interest rate. For example a returned value of 1.84 is 184 %.
		/// </summary>
		/// <returns></returns>
		public static double GetAvgInterestRate()
		{
			//TODO Call IRA Data to get this value here
			return 1.84;
		}

	    public static string ProductionChartCaption(CropType crop)
	    {
            return string.Format( "{0} production (ton)", crop );
        }

	    public static string ProductionCompareChartCaption(CropType crop)
	    {
	        return string.Format("Comparision of {0} biomass production with other crops", crop);
	    }

        public static string CashFlowChartCaption( CropType crop )
        {
            return string.Format( "Cashflow information for {0} biomass production", crop );
        }

        public static string CashFlowCompareChartCaption( CropType crop )
        {
            return string.Format( "Cashflow Comparision of {0} production with other crops", crop );
        }
        public static IEnumerable<SelectListItem> GetState()
		{
			IList<SelectListItem> items = new List<SelectListItem>
			{
				new SelectListItem {Text = @"Alabama", Value = "AL"},
				new SelectListItem {Text = @"Arkansas", Value = "AR"},
				new SelectListItem {Text = @"Florida", Value = "FL"},
				new SelectListItem {Text = @"Georgia", Value = "GA"},
				new SelectListItem {Text = @"Kentucky", Value = "KY"},
				new SelectListItem {Text = @"Mississippi", Value = "MS"},
				new SelectListItem {Text = @"North Carolina", Value = "NC"},
				new SelectListItem {Text = @"South Carolina", Value = "SC"},
				new SelectListItem {Text = @"Tennessee", Value = "TN"},
				new SelectListItem {Text = @"Texas", Value = "TX"},
				new SelectListItem {Text = @"Virginia", Value = "VA"}
			};
			return items;
		}

		/*

			Abbreviation	State Name	Capital	Became a State
AL	Alabama	Montgomery	December 14, 1819
AK	Alaska	Juneau	January 3, 1959
AZ	Arizona	Phoenix	February 14, 1912
AR	Arkansas	Little Rock	June 15, 1836
CA	California	Sacramento	September 9, 1850
CO	Colorado	Denver	August 1, 1876
CT	Connecticut	Hartford	January 9, 1788
DE	Delaware	Dover	December 7, 1787
FL	Florida	Tallahassee	March 3, 1845
GA	Georgia	Atlanta	January 2, 1788
HI	Hawaii	Honolulu	August 21, 1959
ID	Idaho	Boise	July 3, 1890
IL	Illinois	Springfield	December 3, 1818
IN	Indiana	Indianapolis	December 11, 1816
IA	Iowa	Des Moines	December 28, 1846
KS	Kansas	Topeka	January 29, 1861
KY	Kentucky	Frankfort	June 1, 1792
LA	Louisiana	Baton Rouge	April 30, 1812
ME	Maine	Augusta	March 15, 1820
MD	Maryland	Annapolis	April 28, 1788
MA	Massachusetts	Boston	February 6, 1788
MI	Michigan	Lansing	January 26, 1837
MN	Minnesota	Saint Paul	May 11, 1858
MS	Mississippi	Jackson	December 10, 1817
MO	Missouri	Jefferson City	August 10, 1821
MT	Montana	Helena	November 8, 1889
NE	Nebraska	Lincoln	March 1, 1867
NV	Nevada	Carson City	October 31, 1864
NH	New Hampshire	Concord	June 21, 1788
NJ	New Jersey	Trenton	December 18, 1787
NM	New Mexico	Santa Fe	January 6, 1912
NY	New York	Albany	July 26, 1788
NC	North Carolina	Raleigh	November 21, 1789
ND	North Dakota	Bismarck	November 2, 1889
OH	Ohio	Columbus	March 1, 1803
OK	Oklahoma	Oklahoma City	November 16, 1907
OR	Oregon	Salem	February 14, 1859
PA	Pennsylvania	Harrisburg	December 12, 1787
RI	Rhode Island	Providence	May 19, 1790
SC	South Carolina	Columbia	May 23, 1788
SD	South Dakota	Pierre	November 2, 1889
TN	Tennessee	Nashville	June 1, 1796
TX	Texas	Austin	December 29, 1845
UT	Utah	Salt Lake City	January 4, 1896
VT	Vermont	Montpelier	March 4, 1791
VA	Virginia	Richmond	June 25, 1788
WA	Washington	Olympia	November 11, 1889
WV	West Virginia	Charleston	June 20, 1863
WI	Wisconsin	Madison	May 29, 1848
WY	Wyoming	Cheyenne	July 10, 1890
	*/

		/// <summary>
		/// Currenlty switchgrass is hardcoded.
		/// </summary>
		/// <returns></returns>
		public static IEnumerable<SelectListItem> GetStorageMethod( string cropType = "Switchgrass" )
		{

			switch ( cropType )
			{
				case "Switchgrass":
				case "Miscanthus":
					IList<SelectListItem> items = new List<SelectListItem>
					{   //Round
                        new SelectListItem {Text = @"Round Tarp and Pallet", Value = "1"},
						new SelectListItem {Text = @"Round Tarp and Gravel", Value = "2"},
						new SelectListItem {Text = @"Round Tarp on Bare Ground", Value = "3"},
						new SelectListItem {Text = @"Round Pallet No Tarp", Value = "4"},
						new SelectListItem {Text = @"Round Gravel No Tarp", Value = "5"},
						new SelectListItem {Text = @"Round Bare Ground No Tarp", Value = "6"},
                        //Rectangular
                        new SelectListItem {Text = @"Rectangular Bale - Tarp and Pallet", Value = "11"},
						new SelectListItem {Text = @"Rectangular Bale - Tarp and Gravel", Value = "12"},
						new SelectListItem {Text = @"Rectangular Bale - No Tarp", Value = "13"},
						new SelectListItem {Text = @"Rectangular Bale - Gravel No Tarp", Value = "14"}
					};
					return items;
				default:
					return null;
			}
		}


		public static string GetStateName( string statecode )
		{
			switch ( statecode )
			{
				case "AL":
					return "Alabama";
				case "AR":
					return "Arkansas";
				case "FL":
					return "Florida";
				case "GA":
					return "Georgia";
				case "KY":
					return "Kentucky";
				case "MS":
					return "Mississippi";
				case "NC":
					return "North Carolina";
				case "SC":
					return "South Carolina";
				case "TN":
					return "Tennessee";
				case "TX":
					return "Texas";
				case "VA":
					return "Virginia";
				default:
					return statecode;
			}
		}

		public static decimal GetBaleStorageLaborCost( decimal estimate, BaleType baleType, decimal hourlyCost, bool hasTarp = false, bool hasPallet = false )
		{

			int totalLabor = 0;

			if ( hasTarp )
			{
				totalLabor++;
			}

			if ( hasPallet )
			{
				totalLabor++;
			}

			if ( totalLabor == 0 ) //If no tarp or pallet is invovled, the labor cost is 0; 
			{
				return 0;
			}


			decimal baleCount;
			if ( baleType == BaleType.Round )
			{
				baleCount = estimate / WeightBaleRound;
				//  stack = Math.Floor(baleCount/balePerStack);
				//  partial = baleCount%balePerStack;
			}
			else if ( baleType == BaleType.Rectangular )
			{
				baleCount = estimate / WeightBaleRect;
			}
			else
			{
				return 0;

			}

			decimal laborCost = baleCount / BalePerStack * hourlyCost / 2 * totalLabor;     // half and hour to set one stack x total Labor cost (one for tarp and another for pallet)

			return laborCost;
		}
		/// <summary>
		/// 
		/// </summary>
		/// <param name="estimate">Estimated Ton Requirin Storage</param>
		/// <param name="baleType">Round or Rectangular</param>
		/// <param name="annualStorageLandCost">Annual Storage Area Land Rental Cost in $/Acre/Year. No Minimum.</param>
		/// <returns></returns>
		public static decimal GetBaleStorageLandCost( decimal estimate, BaleType baleType, decimal annualStorageLandCost )
		{
			decimal baleCount;
			// double stack = 0;
			//  double partial = 0;
			decimal stroageSqFt;
			if ( baleType == BaleType.Round )
			{
				baleCount = estimate / WeightBaleRound;
				//stack = Math.Floor(baleCount / balePerStack);
				//partial = baleCount % balePerStack;
				stroageSqFt = GravelSqFtPerStackRound;
			}
			else if ( baleType == BaleType.Rectangular )
			{
				baleCount = estimate / WeightBaleRect;
				stroageSqFt = GravelSqFtPerStackRect;
			}
			else
			{
				return 0;
			}
			decimal landCost = baleCount / BalePerStack * stroageSqFt * 1.10m * 0.000022956841138659m * annualStorageLandCost; //expand area of gravel by 10 % X 1/X land Cost per acre

			return landCost;
		}


		public static IList<decimal> GetStorageCost( Input input )
		{


			var cropType = input.General.Category;
			//Return null if crop types are not miscanthus or switchgrass

			if ( CropType.Switchgrass != cropType && CropType.Miscanthus != cropType )
			{
				return null;
			}


			StorageMethod storageMethod =
					input.Storage.StorageMethod;

			var estimate = input.GetAnnualProductivity();
			var cftVolume = estimate / Convert.ToDecimal( ConfigurationManager.AppSettings.Get( "WeightToVolumeRatio" ) );

			var requiresPallet = false;
			var requiresTarp = false;
			BaleType baleType = BaleType.Round;


			//assumtion - 10lbs/cft
			//4 ft diamater, 5 ft height = 62 cft. ft. [pi*D2/4 *H]
			//thus one round bale weigh about 630 lbs
			//one stack cann contain 3780 lbs ir 1.89 lbs
			//one stack needs a tarp of size 15x5 sq. feet
			//75 sq. feet of tarp per 
			//cost of tarp sq. feet .15 or $11.25
			// $5.95/ton


			//every 72 months
			//2% additional cost
			//minimum 1 bale

			//if production is greater than 3780 one rate
			//if production is less than 3780 then another rate
			//for round

			//For rectangular
			//8 x 5 ft. x 5 = 200 cft
			//when stacked = 1200 cft.
			//or 12000 lbs = 6 tons
			//needs about 200 sq. ft of tarp [40 ft long and 5 ft wide for one stack] or $30
			//tarp cost per ton = $5/ton

			//Fixed cost
			//Pallet cost - about 25 cents per sq. ft. or $ 15 per stack for round and $30 per stack rect.
			//http://www.recycle.net/cgi-bin/exview.cgi?w=01&sc=1101&st=LA
			//http://ag-econ.ncsu.edu/sites/ag-econ.ncsu.edu/files/extension/budgets/forage/ForageBudHayFromStoragePrint08-848.pdf
			//Gravel cost
			//.75 cents sq foot

			//81 per 6 bales
			//27 per bale

			decimal annualStorageCost;


			decimal baleCount = estimate / WeightBaleRound;
			decimal stack = Math.Floor( baleCount / BalePerStack );
			decimal partial = baleCount % BalePerStack;

			decimal baleCountRect = estimate / WeightBaleRect;
			decimal stackRect = Math.Floor( baleCountRect / BalePerStack ); //bale per stack is always 6.
			decimal partialRect = baleCountRect % BalePerStack;

			decimal tarpSqFtCost = input.Storage.TarpCost;
			decimal gravelSqFtCost = input.Storage.GravelCost;
			decimal palletSqFtCost = input.Storage.PalletCost;



			//labor costs are fixed. No labor for gravel setting. Half an hour cost for tarp and pallet
			//Land costs are Annual

			decimal gravelCostRound = stack * GravelSqFtPerStackRound * gravelSqFtCost + partial * SqFtPerBaleRound * gravelSqFtCost;
			decimal palletCostRound = stack * PalletSqFtPerStackRound * palletSqFtCost + partial * SqFtPerBaleRound * palletSqFtCost;
			decimal tarpCostRound = stack * TarpSqFtPerStackRound * tarpSqFtCost + partial * SqFtPerBaleRound * tarpSqFtCost;

			decimal gravelCostRect = stackRect * GravelSqFtPerStackRect * gravelSqFtCost + partialRect * SqFtPerBaleRect * gravelSqFtCost;
			decimal palletCostRect = stackRect * PalletSqFtPerStackRect * palletSqFtCost + partialRect * SqFtPerBaleRect * palletSqFtCost;
			decimal tarpCostRect = stackRect * TarpSqFtPerStackRect * tarpSqFtCost + partialRect * SqFtPerBaleRect * tarpSqFtCost;


			decimal oneTimeCost = 0;
			IList<decimal> annualizedStorageCost = null;

			if ( input.General != null )
			{
				annualizedStorageCost = new decimal[input.General.ProjectLife.GetValueOrDefault()];
			}
			else
				annualizedStorageCost = new decimal[10];
			//If user has supplied flat cost, then storage cost per year from user is used.
			if ( input.Storage.CostOption == (int)StorageCostEstimationOption.UserSupplyStorageCost )
			{
				annualStorageCost = input.Storage.UserEstimatedCost;

				for ( int i = 0; i < annualizedStorageCost.Count(); i++ )
				{
					annualizedStorageCost[i] = annualStorageCost;
				}

				return annualizedStorageCost;
			}

			if ( input.Storage.CostOption == (int)StorageCostEstimationOption.Default || input.Storage.CostOption == (int)StorageCostEstimationOption.UserSupplyMaterialCost )
			{

				if ( ( CropType.Switchgrass == cropType ) || ( CropType.Miscanthus == cropType ) )
				{
					switch ( storageMethod )
					{
						case StorageMethod.RoundTarpPallet:
							{
								oneTimeCost = palletCostRound + tarpCostRound;
								requiresTarp = true;
								requiresPallet = true;
								break;
							}

						case StorageMethod.RoundTarpGravel:
							oneTimeCost = gravelCostRound + tarpCostRound;
							requiresTarp = true;
							requiresPallet = true;
							break;
						case StorageMethod.RoundTarpBareGround:
							oneTimeCost = tarpCostRound;
							requiresTarp = true;
							requiresPallet = false;
							break;
						case StorageMethod.RoundPalletNoTarp:
							oneTimeCost = palletCostRound;
							requiresTarp = false;
							requiresPallet = true;
							break;
						case StorageMethod.RoundGravelNoTarp:
							oneTimeCost = gravelCostRound;
							requiresTarp = false;
							requiresPallet = false;
							break;
						case StorageMethod.RoundBareGroundNoTarp:
							oneTimeCost = gravelCostRound;
							requiresTarp = false;
							requiresPallet = false;

							break;
						case StorageMethod.RectangularTarpPallet:
							oneTimeCost = palletCostRect + tarpCostRect;
							requiresTarp = true;
							requiresPallet = true;
							baleType = BaleType.Rectangular;
							break;
						case StorageMethod.RectangularNoTarp:
							oneTimeCost = 0;
							requiresTarp = false;
							requiresPallet = false;
							baleType = BaleType.Rectangular;
							break;
						case StorageMethod.RectangularGravelNoTarp:
							oneTimeCost = gravelCostRect;
							requiresTarp = false;
							requiresPallet = false;
							baleType = BaleType.Rectangular;
							break;
					}
					annualizedStorageCost = GetAnnualStorageCost( oneTimeCost, input.General.ProjectLife.GetValueOrDefault() );

					//Add land and labor cost
					var annualProduction = input.GetAnnualProductionList();

					for ( int i = 0; i < annualizedStorageCost.Count(); i++ )
					{
						decimal laborCost = GetBaleStorageLaborCost( (decimal)( annualProduction[i] * input.Storage.PercentStored / 100 ), baleType, (decimal)input.Storage.LaborCost.GetValueOrDefault(), requiresTarp, requiresPallet );
						decimal landCost = GetBaleStorageLandCost( (decimal)( annualProduction[i] * input.Storage.PercentStored / 100 ), baleType, (decimal)input.Storage.LandCost.GetValueOrDefault() );

						annualizedStorageCost[i] = annualizedStorageCost[i] + laborCost + landCost;
					}
					return annualizedStorageCost;

				}
			}

			return annualizedStorageCost;
		}

		private static IList<decimal> GetAnnualStorageCost( decimal oneTimeCost, int projectLife = 10 )
		{

			IList<decimal> annualizedStorageCost = new decimal[projectLife];
			//First year one time cost
			//year 2 - 5, put two percent of the annual cost
			//every 6th years, annualCost + 8% and then continue to two percent
			var tempOneTimeCost = oneTimeCost;
			const decimal percentIncrement = 0.02m;
			for ( int i = 0; i < projectLife; i++ )
			{
				if ( i == 0 )
				{
					annualizedStorageCost[i] = oneTimeCost;
				}
				else if ( i > 0 && i % 5 == 0 )
				{
					tempOneTimeCost = tempOneTimeCost * ( 1 + percentIncrement * i );
					annualizedStorageCost[i] = tempOneTimeCost;

				}
				else if ( i > 0 && i % 5 != 0 )
				{
					annualizedStorageCost[i] = tempOneTimeCost * percentIncrement;
				}

			}

			return annualizedStorageCost;

		}

		public static decimal GetStorageLoss( int storageMethod, CropType cropType = CropType.Switchgrass )
		{
			var result = 0.0m;


			if ( cropType == CropType.Switchgrass || cropType == CropType.Miscanthus )
			{

				switch ( storageMethod )
				{
					case 1:
						result = 1.0m;
						break;
					case 2:
						result = 8.5m;
						break;
					case 3:
						result = 7.0m;
						break;
					case 4:
						result = 18.2m;
						break;
					case 5:
						result = 16.6m;
						break;
					case 6:
						result = 12.8m;
						break;
					case 11:
						result = 13.7m;
						break;
					case 12:
						result = 28.0m;
						break;
					case 13:
						result = 48.0m;
						break;
					case 14:
						result = 57.1m;
						break;
					default:
						result = 0m;
						break;
				}

			}
			return result;
		}


		public static Double GetStorageLoss( int storageMethod, string cropType = null )
		{
			var result = 0.0;
			if ( cropType == null )
			{
				cropType = "SwitchGrass";
			}

			if ( cropType.ToLower().Equals( "switchgrass" ) || cropType.ToLower().Equals( "miscanthus" ) )
			{

				switch ( storageMethod )
				{
					case 1:
						result = 1.0;
						break;
					case 2:
						result = 8.5;
						break;
					case 3:
						result = 7.0;
						break;
					case 4:
						result = 18.2;
						break;
					case 5:
						result = 16.6;
						break;
					case 6:
						result = 12.8;
						break;
					case 11:
						result = 13.7;
						break;
					case 12:
						result = 28.0;
						break;
					case 13:
						result = 48.0;
						break;
					case 14:
						result = 57.1;
						break;
					default:
						result = 0;
						break;
				}

			}

			return result;
		}

		public static Tuple<string, string, string> GetImageUrl(CropType category)
		{
			switch ( category )
			{
				case CropType.Switchgrass:
					return new Tuple<string,string, string>("http://www.ars.usda.gov/is/graphics/photos/apr07/d750-1.jpg", 
                        "Bioenergy crop switchgrass (Photo by: Peggy Greb).",
                        "~/images/switchgrass_pdf.jpg");
				case CropType.Miscanthus:
					return new Tuple<string, string, string>( "http://www.ethanolproducer.com/uploads/posts/web/2014/10/UofIMiscanthus_14147108585304.jpg", 
                        "Bioenergy crop Miscanthus (Photo by: Brian Stauffer)" ,
                        "~/images/UofIMiscanthus_14147108585304.jpg");

                case CropType.Poplar:
					return new Tuple<string, string, string>( "http://learn.forestbioenergy.net/learning-modules/module-2/unit-3/large%20cottonwood.jpg",
						"Poplar tree plantation (Photo by: Warren Gretz)",
                        "~/images/largecottonwood.jpg");
				case CropType.Willow:
					return new Tuple<string, string, string>( "https://articles.extension.org//sites/default/files/Coppice%20Regrowth%20at%20Belleville%20299%20x%20200.jpg", 
                        "Willow biomass crops, is about a month old above ground on a four year old root system. " +
                        "(Photo By:  T. Volk).",
                        "~/images/CoppiceRegrowthAt_Belleville299x200.jpg");  
				case CropType.Pine:
					return new Tuple<string, string, string>( "http://www.srs.fs.usda.gov/compass/wp-content/uploads/2013/09/09.18.-Planted-lob-35yrs.jpg", 
                        "Planted loblolly pine stand (Photo by: David Stephens)" ,
                        "~/images/Planted-lob-pine-35yrs.jpg");

				default:
                    return
						new Tuple<string, string, string>(
							"http://www.climatetechwiki.org/sites/climatetechwiki.org/files/imagecache/Illustration/images/teaser/agri_for_biofuel_teaser_image.jpg",
							"", "~/images/CoppiceRegrowthAt_Belleville299x200.jpg");



			}
			

		}
	}
}