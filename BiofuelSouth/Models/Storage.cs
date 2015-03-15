using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Models
{
    public class Storage
    {
        public Storage()
        {
            RequireStorage = false;
            StorageTime = 0.0;
            PercentDirectlyToPlantGate = 0.0;
            PercentStored = 0.0;
            
        }

        public bool RequireStorage { get; set; } //true if storage is required, false if not.

        [Range(0, 200)]
        [DisplayName(@"Storage Time in Days (Upto 200 days)")]
        public Double StorageTime { get; set; } //number of days, Max 200 days;
        [Range(0,100)]
        [DisplayName(@"Percentage Going Directly to Plantgate")]
        public Double PercentDirectlyToPlantGate { get; set; }
        [Range(0, 100)]
        [DisplayName(@"Percentage Requiring Storage")]
        public Double PercentStored { get; set; } //proportion requiring storage
         [DisplayName(@"Storage Method")]
        public String StorageMethod { get; set; } //Storage methods.
    }
}