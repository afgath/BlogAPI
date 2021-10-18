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
        [DefaultValue(0)]
        public decimal CommentId { get; set; }
        [Required]
        [MaxLength(500), MinLength(1)]
        public string Contents { get; set; }
        [Required]
        public decimal CreationUserId { get; set; }
        [Required]
        public decimal PostId { get; set; }
        public decimal? Likes { get; set; }
        public decimal? Dislikes { get; set; }
    }
}
