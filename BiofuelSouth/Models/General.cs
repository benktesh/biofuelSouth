using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiofuelSouth.Models
{
    public class General
    {
        public int Id { get; set; }

        [Required]
        public String State { get; set; }

        [Required]
        [DisplayName(@"Name of County")]
        public string County { get; set; }


        [Required]
        [DisplayName(@"Biofuel Category")]
        public String Category { get; set; }

        [DisplayName(@"Size of Project (acre)")]
        public double ProjectSize { get; set; }

        [DisplayName(@"Years (From plantation to harvest")]
        public int ProjectLife { get; set; }  //years

        [DisplayName(@"Farm Gate Price ($/dry ton)")]
        public double BiomassPriceAtFarmGate { get; set; } //$/ton


        [DisplayName(@"Cost of land ($/acre/year)")]
        public double LandCost { get; set; } //$/acre/year

    }
}