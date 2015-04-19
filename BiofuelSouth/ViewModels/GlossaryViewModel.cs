using System;
using System.ComponentModel.DataAnnotations;

namespace BiofuelSouth.ViewModels
{
    public class GlossaryViewModel
    {
        [Required]
        [MinLength(1, ErrorMessage = @"Must not be empty.")]
        public string Term { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = @"Must be a reasonable word of at least three characters.")]
        public string Keywords { get; set; }
        [Required]
        [MinLength(10, ErrorMessage = @"Must be of at least 10 characters long.")]
        public string Description { get; set; }
        [Required]
        [MinLength(3, ErrorMessage = @"Must be a reasonable word of at least three characters.")]
        public string Source { get; set; }

        public Guid AdminToken { get; set;  }

    }
}