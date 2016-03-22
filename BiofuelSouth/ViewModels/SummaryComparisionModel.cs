using BiofuelSouth.Enum;

namespace BiofuelSouth.ViewModels
{
    public class SummaryComparisionModel
    {
        public ResultComparisionKey Key { get; set; }
        public CropType Crop { get; set; }

        public string ComparisionValue { get; set; }

    }
}