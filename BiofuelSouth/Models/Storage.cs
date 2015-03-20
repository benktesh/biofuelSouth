using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Models
{
    public class Storage
    {
        public Storage()
        {
          
            StorageTime = 0.0;
            PercentDirectlyToPlantGate = 0.0;
            PercentStored = 0.0;
            
        }

        [Required]
        [DisplayName(@"Require Storage?")]
        public bool? RequireStorage { get; set; } //true if storage is required, false if not.

        [Range(0, 200)]
        [DisplayName(@"Storage Days")]
        [Required]
        public Double StorageTime { get; set; } //number of days, Max 200 days;
        [Range(0,100)]
        [DisplayName(@"Percentage Going Directly to Plantgate")]
        public Double PercentDirectlyToPlantGate { get; set; }
        [Range(0, 100)]
        [Required]
        [DisplayName(@"Percent Requiring Storage")]
        public Double PercentStored { get; set; } //proportion requiring storage
         [DisplayName(@"Method")]
         [Required]
        public String StorageMethod { get; set; } //Storage methods.
    }
}