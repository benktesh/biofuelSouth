using System.Collections.Generic;
using BiofuelSouth.Enum;

namespace BiofuelSouth.Services
{
    public static class CropAttribute
    {
        public static int GetRoationYears(CropType crop)
        {
            switch (crop)
            {
                case CropType.Switchgrass:
                    return 1;
                case CropType.Miscanthus:
                    return 1;
                case CropType.Pine:
                    return 10;
                case CropType.Poplar:
                    return 3;
                case CropType.Willow:
                    return 3;
                default:
                    return 10;
            }
        }

        public static int? GetThinningYear(CropType crop)
        {
            if (crop == CropType.Pine)
            {
                return 5;
            }
           return null; 
        }

        public static bool CanCoppice(CropType cropType)
        {
            switch (cropType)
            {
                case CropType.Switchgrass:
                case CropType.Miscanthus:
                case CropType.Pine:
                case CropType.Willow:
                    return false;
                case CropType.Poplar:
                    return true; 
                default:
                    return false;
            }

        }
        public static IList<double> GetProductivityTaper(CropType cropType)
        {
            switch (cropType)
            {
                case CropType.Switchgrass:
                case CropType.Miscanthus:
                    return new List<double> { 0.25, 0.5, 1 };
                case CropType.Pine:
                    return new List<double> { 0.80, 1 };
                case CropType.Poplar:
                case CropType.Willow:
                    return new List<double> { 0.80, 1 };

                default:
                    return new List<double> { 1 };
            }
        }

    }
}