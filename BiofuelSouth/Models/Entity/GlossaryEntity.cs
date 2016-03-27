using System;
using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Models.Entity
{

    public class GlossaryEntity
    {

        [Key]
        public Guid Id { get; set; }
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
      

        public GlossaryEntity(String t, String k, String d, String s)
        {
            Term = t;
            Keywords = k;
            Description = d;
            Source = s;
            Counter = 0; 
            Id = Guid.NewGuid();
        }

        public GlossaryEntity()
        {
            Id = Guid.NewGuid();
        }



    }
}
