using System.ComponentModel.DataAnnotations;
using System;


namespace BiofuelSouth.Models
{

    public class Glossary
    {
        
        public string Term { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; } 
        public int? Counter { get; set; }
        public string Source { get; set; }
        public string ModifiedBy { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Modified { get; set; }
        public int? IsDirty { get; set;  }
        [Key]
        public Guid Id { get; set; }

        public Glossary(String t, String k, String d, String s)
        {
            Term = t;
            Keywords = k;
            Description = d;
            Source = s;
            Counter = 0; 
            Id = Guid.NewGuid();
        }

        public Glossary()
        {
            Id = Guid.NewGuid();
        }



    }
}
