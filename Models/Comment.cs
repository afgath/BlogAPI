using System;
using System.Collections.Generic;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class Comment
    {
        public decimal CommentId { get; set; }
        public string Contents { get; set; }
        public decimal CreationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsReview { get; set; }
        public decimal PostId { get; set; }
        public decimal? Likes { get; set; }
        public decimal? Dislikes { get; set; }
        public int Status { get; set; }

        public virtual Post Post { get; set; }
        public virtual User CreationUser { get; set; }
    }
}
