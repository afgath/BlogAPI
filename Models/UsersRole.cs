using System;
using System.Collections.Generic;

#nullable disable

namespace zmgTestBack.Models
{
    public partial class UsersRole
    {
        public decimal UsersRolesId { get; set; }
        public decimal UserId { get; set; }
        public decimal RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
