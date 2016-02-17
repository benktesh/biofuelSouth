using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using BiofuelSouth.Enum;
using BiofuelSouth.Resources;

namespace BiofuelSouth.Models
{
    public class General
    {
        public int Id { get; set; }

        [Required]
        [Tooltip("Name of State")]
        public String State { get; set; }

        [Required]
        [DisplayName(@"County")]
        public string County { get; set; }

        [Required]
        [DisplayName(@"Biomass Crop")]
        public CropType Category { get; set; }

        [DisplayName(@"Project Size (acre)")]
        [Required]
        public double? ProjectSize { get; set; }

        [DisplayName(@"Project Duration (Years)")]
        [Range(1, 100)]
        [Required]
        public int? ProjectLife { get; set; }  //years

        [DisplayName(@"Farm Gate Price ($/dry ton)")]
        [Required]
        public decimal? BiomassPriceAtFarmGate { get; set; } //$/ton


        [DisplayName(@"Land Cost ($/acre/year)")]
        [Required]

        public decimal? LandCost { get; set; } //$/acre/year

        public IEnumerable<SelectListItem> CountyList { get; set; }
        public IEnumerable<SelectListItem> StateList { get; set; }

        public WizardStep CurrentStep { get; set; }

        public string PreviousActionn
        {
            get { return "General"; }
        }

        public General()
        {
            ProjectLife = 10;
            LandCost = 80;

        }
    }
}