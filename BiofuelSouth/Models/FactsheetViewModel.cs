using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BiofuelSouth.Enum;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace BiofuelSouth.Models
{
    public class FactsheetViewModel
    {

        public CropType CropType { get; set;  }

        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Description { get; set; }

        public bool CanPDFFactsheet { get; set; }


		[Display( Name = @"Species Distribution" )]
		public string SpeciesDescription { get; set; }


		[Display(Name = @"Geographic Distribution")]
        public string GeographicDistribution { get; set;  }

        [Display(Name = @"Available Crops")]
        public List<CropType> AvailableCrops { get; set; }
        [Display(Name = @"Production Process")]
        public string ProductionProcess { get; set; }
        public string Yield { get; set; }

        [Display(Name= @"Facts For Quick Reference")]
        public Dictionary<string, string> FactsForQuickReference { get; set; }
		public string FactsForQuickReferenceTitle { get; set;  }

		public Dictionary<string, string> Heatingvaluesummary { get; set;  }
		public string HeatingValueSummaryTitle { get; set;  }


		[Display( Name = @"Glossary of Common Terms" )]
		public Dictionary<string, string> GlossaryOfCommonTerms { get; set; }
	    public string GlossaryOfCommontermsTitle { get; set; }
		public bool ContainsGlossaryHeading { get; set; }



		[Display(Name = @"Prepared By")]
        public string PreparedBy { get; set; }

        public string Conclusion { get; set; }

        [Display(Name = @"Growth Rate")]
        public string GrowthRate { get; set; }

        [Display(Name=@"Adapted From")]
        public string AdaptedFrom { get; set; }


        public List<string> References { get; set; }

        [Display(Name = @"Other Sources")]
        public List<string> OtherSources { get; set; }

        public FactsheetViewModel()
        {
            AvailableCrops = System.Enum.GetValues(typeof (CropType)).Cast<CropType>().ToList();
            
        }
    }
}