using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public class Glossary
    {
        [Key]
        public string term { get; set; }
        public string keywords { get; set; }
        public string description { get; set; } 
        public int? counter { get; set; }
        public string source { get; set; }

        public Glossary(String t, String k, String d, String s)
        {
            this.term = t;
            this.keywords = k;
            this.description = d;
            this.source = s;
            this.counter = 0; 
        }

        public Glossary()
        {
          
        }


    }
}
