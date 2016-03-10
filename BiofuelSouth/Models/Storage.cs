using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using BiofuelSouth.Enum;

namespace BiofuelSouth.Models
{
    public class Storage : ICloneable
    {
        public Storage()
        {
            StorageTime = 0.0;
            PercentDirectlyToPlantGate = 0.0;
            PercentStored = 0.0;
            GravelCost = StorageCostParameter.GravelCostSqFt;
            TarpCost = StorageCostParameter.TarpCostSqFt;
            PalletCost = StorageCostParameter.PalletCostSqFt;
            LandCost = (double) StorageCostParameter.LandCostYear;
            LaborCost = (double) StorageCostParameter.LaborCostHour;
        }

        public WizardStep CurrentStep { get; set; }

        public string PreviousAction { get; set; }

        [Required]
        [DisplayName(@"Require Storage?")]
        public bool? RequireStorage { get; set; } //true if storage is required, false if not.

        [Range(0, 200)]
        [DisplayName(@"Storage Days")]
        [Required]
        public Double StorageTime { get; set; } //number of days, Max 200 days;
        [Range(0,100)]
        [DisplayName(@"Percentage Going Directly to Plantgate")]
        public Double PercentDirectlyToPlantGate { get; set; }
        [Range(0, 100)]
        [Required]
        [DisplayName(@"Percent Requiring Storage")]
        
        
        public Double PercentStored { get; set; } //proportion requiring storage
        [DisplayName(@"Method")]
        [Required]
        public StorageMethod StorageMethod { get; set; } //Storage methods.

         [Required]
        [DisplayName(@"Select Cost Parameters")]
         public int CostOption { get; set;  }

        //default values needs to be supplied.
         [Display(Name = @"Storage Cost Assumptions")]
         public String Description { get; set; }

         [Display(Name = @"Cost of Pallet ($/sq.ft)")]
         public Decimal PalletCost { get; set; }

         [Display(Name = @"Cost of Tarp ($/sq.ft)")]
         public Decimal TarpCost { get; set; }

         [Display(Name = @"Cost of Gravel ($/sq.ft)")]
         public Decimal GravelCost { get; set; }

         [Display(Name = @"Labor cost ($/hour)")]
         public double? LaborCost { get; set; }

         [Display(Name = @"Land cost ($/ac/year")]
         public double? LandCost { get; set; }

         [Display(Name = @"Enter estimated storage cost in $/ton/year")]
         public Decimal UserEstimatedCost { get; set; }

	    public object Clone()
	    {
		    Storage newStorage = (Storage) this.MemberwiseClone();
			return newStorage;
	    }
    }
}