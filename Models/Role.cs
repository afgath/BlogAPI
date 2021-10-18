using System;
using System.Collections.Generic;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class Role
    {
        public Role()
        {
            UsersRoles = new HashSet<UsersRole>();
        }

        public decimal RoleId { get; set; }
        public string RoleName { get; set; }

        public virtual ICollection<UsersRole> UsersRoles { get; set; }
    }
}
