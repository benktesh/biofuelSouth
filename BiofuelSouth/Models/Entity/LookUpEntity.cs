using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BiofuelSouth.Models.Entity
{
    public class LookUpEntity
    {
        [Key]
        public Guid Id { get; set;  }
        public int LookUpId { get; set; }
        public string Name { get; set; }
        public string Description { get; set;  }
        public string Source { get; set; }
        public string Label { get; set; }
        public string Value { get; set; }
        public string LookUpGroup { get; set; }
        public int SortOrder { get; set; }
        public bool System { get; set; }
    }
}