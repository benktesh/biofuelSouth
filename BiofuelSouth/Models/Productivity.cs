﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiofuelSouth.Models
{
    public class Productivity
    {
        public int Id { get; set; }
        public int CountyId { get; set; }
        public String CropType { get; set; }
        public Double Yield { get; set; }
        public Double Cost { get; set; }


    }
}