using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BiofuelSouth.Enum;
using BiofuelSouth.Models;

namespace BiofuelSouth.ViewModels
{
    public class StorageCostModel
    {
        public StorageMethod storageMethod { get; set; }
        
        public CropType CropType { get; set; }

        [Display(Name = @"Storage Cost Assumptions")]
        public String Description { get; set; }

        [Display(Name=@"Cost of Pallet ($/sq.ft")]
        public Decimal PalletCost { get; set; }

        [Display(Name = @"Cost of Tarp ($/sq.ft")]
        public Decimal TarpCost { get; set; }

        [Display(Name = @"Cost of Gravel ($/sq.ft")]
        public Decimal GravelCost { get; set; }

        [Display(Name = @"Local labor cost for storage($/sq.ft")]
        public Decimal LaborCost { get; set; }

        [Display(Name = @"Land cost of storage. Include any additional cost of sheds.($/sq.ft")]
        public Decimal LandCost { get; set; }

        [Display(Name = @"Local labor cost for storage($/ton/day")]
        public Decimal EstimatedCost { get; set; }
    }
}