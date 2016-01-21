using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using BiofuelSouth.Enum;

namespace BiofuelSouth.Models
{

    public class ProductionCost
    {
        public ProductionCostType ProductionCostType { get; set; }

        public int? ImplementationYearOnCycle { get; set; }

        public bool IsRequired { get; set; }

        [Display(Name = @"Production Cost ($/acre)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? Amount { get; set; }

        [Display(Name = @"Unit")]
        public string Unit { get; set; }

        public ProductionCost()
        {
            Unit = "$/acre";
        }

    }
    public class ProductionCostViewModel
    {
        public CropType CropType { get; set; }

        public string County { get; set; }

        [Display(Name = @"Production Cost ($/acre)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal? Amount { get; set; }

        [Display(Name = @"Customize Cost")]
        public bool UseCustom { get; set; }

        public bool CanCustomize
        {
            get
            {
                if (CropType == CropType.Miscanthus || CropType == CropType.Switchgrass)
                {
                    return false;
                }
                return true;
            }
        }

        public WizardStep CurrentStep { get; set; }

        public List<ProductionCost> ProductionCosts { get; set; }

        public ProductionCostViewModel()
        {
            //CropType = CropType.Switchgrass;
            ProductionCosts = new List<ProductionCost>();
        }

        public decimal TotalProductionCost
        {
            get
            {
                if (UseCustom && ProductionCosts.Any())
                {

                    decimal amount = ProductionCosts.Aggregate<ProductionCost, decimal>(0,
                        (current, c) => current + c.Amount.GetValueOrDefault());
                    return amount;
                }

                if (Amount != null) return (decimal) Amount;
                return 0;
            }
        }
    }
}
