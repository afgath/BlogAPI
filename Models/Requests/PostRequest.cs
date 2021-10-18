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
        public decimal CreationUserId { get; set; }
        public decimal? ModificationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal? Views { get; set; }
        public int Status { get; set; }
        public decimal? Likes { get; set; }
        public decimal? Dislikes { get; set; }

    }
}
