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
        
    }
}