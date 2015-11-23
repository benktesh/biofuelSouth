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

        public static List<string> GetRotationYearInstruction(CropType crop)
        {
            var resultString = new List<string>();
            switch (crop)
            {
                case CropType.Switchgrass:
                    resultString.Add(
                        "Switchgrass is harvested annually for the next years during which crop regrows from coppice. " +
                        "New stock is planted after 10 harvests.");
                    break;
                case CropType.Miscanthus:
                    resultString.Add(
                        "Miscanthus is harvested annually for the next years during which crop regrows from coppice. " +
                        "New stocks is planted after 10 harvests.");
                    break;
                case CropType.Pine:
                    resultString.Add("Pine is harvested every 10 - 12 years. " +
                                     " and new plantation is established at every harvest.");
                    break;
                case CropType.Poplar:
                    resultString.Add("Poplar is harvested every 3 years and crop regrows from coppice. " +
                                     "New stocks are planted after 6 or 7 harvests.");
                    break;
                case CropType.Willow:
                    resultString.Add("Willow is harvested every 3 years and crop regrows from coppice. " +
                                     "New stocks are planted after 6 or 7 harvests.");
                    break;
                default:
                    resultString.Add("This crop is assumed to be harvested every 10 years." +
                                     "New stocks are planted after 6 or 7 harvests.");
                    break;
            }
            return resultString;

        }

    }
}