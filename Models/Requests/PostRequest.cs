using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class PostRequest
    {

        [Required]
        public decimal PostId { get; set; }
        [Required]
        [MaxLength(200),MinLength(1)]
        public string Title { get; set; }
        [Required]
        [MinLength(1)]
        public string Contents { get; set; }

    }
}
