using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class CommentRequest
    {
        [Required]
        [MaxLength(500), MinLength(1)]
        public string Contents { get; set; }
        [Required]
        public decimal PostId { get; set; }
    }
}
