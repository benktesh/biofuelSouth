using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Enum
{
    public enum CropType
    {
        Switchgrass,
        Miscanthus,
        Poplar,
        Willow,
        Pine
           
    }

    public enum StorageCostEstimationOption
    {
        [Display(Name=@"Default")]
        Default = 0,
        [Display(Name = @"Use Lumpsum Storage Cost")]
        UserSupplyStorageCost = 1,
        [Display(Name = @"Derive from Supplied Materials Cost")]
        UserSupplyMaterialCost = 2
    }

    public enum BaleType
    {
        Round,
        Rectangular
    }
}