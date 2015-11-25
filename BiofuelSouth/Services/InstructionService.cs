using System.Collections.Generic;
using BiofuelSouth.Enum;

namespace BiofuelSouth.Services
{
    public static class InstructionService
    {
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


        private static List<string> GetDefaultFinancialInstruction()
        {
            var resultString = new List<string>();
            resultString.Add(
                       "If financing (such as loan or assistance) is not required or not applicable to the project," +
                       "this step can be skipped");

            resultString.Add(
                "When a loan is applicable, the project cost is likely to increase due to interest on payment. On the " +
                "other hand, when assistance is avialable, the project cost may decrease depending on type of assistance" +
                "available.");

            resultString.Add(
               "Loan may be for entire project cost or the part of the project cost.");


            resultString.Add(
                "Any cost related the project that has not been covered in previous steps can be added to 'Administrative Cost'. This cost" +
                "may include costs like loan charge (but not the interest), management costs, extension cost, marketting cost etc");

            return resultString;

        }
        public static List<string> GetFinancialInstruction(CropType? crop = null)
        {
            var resultString = GetDefaultFinancialInstruction();

            if (crop != null)
            {
                switch (crop)
                {
                    case CropType.Switchgrass:

                        break;
                    case CropType.Miscanthus:

                        break;
                    case CropType.Pine:

                        break;
                    case CropType.Poplar:

                        break;
                    case CropType.Willow:

                        break;
                }
            }

            resultString.Add("Locally available accurate values can be used instead of defaults in all cases.");
            return resultString;

        }
    }
}