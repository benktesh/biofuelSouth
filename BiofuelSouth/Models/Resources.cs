namespace BiofuelSouth.Models
{
    public static class Resources
    {
       
    }

    public static class StorageCostParameter
    {
        public static decimal TarpCostSqFt = (decimal)   0.15;
        public static decimal GravelCostSqFt = (decimal) 0.75;
        public static decimal PalletCostSqFt = (decimal) 0.25;
        public static decimal LaborCostHour = (decimal) 10.00;
        public static decimal LandCostYear = (decimal)  70.00;
    }

    public static class LookupGroup
    {

        public const string StorageCostParameter = "storage-cost-parameter";

    }
}