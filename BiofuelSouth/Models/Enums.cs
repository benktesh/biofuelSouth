using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiofuelSouth.Models
{
    public enum CropType
    {
        Switchgrass,
        Miscanthus,
        YelloPoplar,
        Willow
    }

    public enum CostEstimationOption
    {
        Default = 0,
        UserSupplyStorageCost = 1,
        UserSupplyMaterialCost = 2
    }
}