using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
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
        [DisplayName(@"Name of County")]
        public string County { get; set; }

        [Required]
        [DisplayName(@"Biofuel Category")]
        public CropType Category { get; set; }

        [DisplayName(@"Size of Project (acre)")]
        [Required]
        public double? ProjectSize { get; set; }

        [DisplayName(@"Years (From plantation to harvest)")]
        [Range(1,100)]
        [Required]
        public int? ProjectLife { get; set; }  //years

        [DisplayName(@"Farm Gate Price ($/dry ton)")]
        [Required]
        public double? BiomassPriceAtFarmGate { get; set; } //$/ton


        [DisplayName(@"Cost of land ($/acre/year)")]
        [Required]
        
        public double? LandCost { get; set; } //$/acre/year

        public IEnumerable<SelectListItem> CountyList { get; set;  }
        public IEnumerable<SelectListItem> StateList { get; set; }

        public General()
        {
            ProjectLife = 10;
            LandCost = 80;
        }
    }
}