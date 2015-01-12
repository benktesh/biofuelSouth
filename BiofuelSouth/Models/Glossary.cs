using System.ComponentModel.DataAnnotations;
using System;


namespace BiofuelSouth.Models
{

    public class Glossary
    {
        [Key]
        public string Term { get; set; }
        public string Keywords { get; set; }
        public string Description { get; set; } 
        public int? Counter { get; set; }
        public string Source { get; set; }

        public Glossary(String t, String k, String d, String s)
        {
            Term = t;
            Keywords = k;
            Description = d;
            Source = s;
            Counter = 0; 
        }

        public Glossary()
        {
          
        }


    }
}
