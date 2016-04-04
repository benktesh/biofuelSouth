using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BiofuelSouth.Enum;
using System; 



namespace BiofuelSouth.Models
{
    public class FactsheetViewModel
    {

        public CropType CropType { get; set;  }
        
        public List<CropType> AvailableCrops { get; set; }

        public FactsheetViewModel()
        {
            AvailableCrops = System.Enum.GetValues(typeof (CropType)).Cast<CropType>().ToList();
        }
    }
}