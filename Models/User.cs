using System;
using System.Collections.Generic;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class User
    {
        public User()
        {
            PostCreationUsers = new HashSet<Post>();
            PostModificationUsers = new HashSet<Post>();
            UsersRoles = new HashSet<UsersRole>();
        }

        public decimal UserId { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Post> PostCreationUsers { get; set; }
        public virtual ICollection<Post> PostModificationUsers { get; set; }
        public virtual ICollection<UsersRole> UsersRoles { get; set; }

        public virtual ICollection<Comment> CommentCreationUsers { get; set; }
    }
}
