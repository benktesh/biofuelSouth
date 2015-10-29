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

    public enum CostEstimationOption
    {
        Default = 0,
        UserSupplyStorageCost = 1,
        UserSupplyMaterialCost = 2
    }

    public enum BaleType
    {
        Round,
        Rectangular
    }

    public enum WizardStep
    {
        General,
        ProductionCost,
        StorageCost,
        Financial,
        Result,
        PostResult
    }
}