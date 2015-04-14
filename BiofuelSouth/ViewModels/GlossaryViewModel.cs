using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BiofuelSouth.ViewModels
{
    public class GlossaryViewModel
    {
        public string Term { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; } 
        public string Source { get; set; }
        public Guid AdminToken { get; set;  }

    }
}