using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
        }

        public decimal PostId { get; set; }
        public string Title { get; set; }
        public string Contents { get; set; }
        public decimal CreationUserId { get; set; }
        public decimal? ModificationUserId { get; set; }
        public DateTime CreationDate { get; set; }
        public DateTime? ModificationDate { get; set; }
        public decimal? Views { get; set; }
        public int Status { get; set; }
        public decimal? Likes { get; set; }
        public decimal? Dislikes { get; set; }

        public virtual User CreationUser { get; set; }
        public virtual User ModificationUser { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
