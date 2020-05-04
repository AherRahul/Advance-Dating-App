using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace Dating.API.Models
{
    public class Role : IdentityRole<int>
    {
        public virtual ICollection<UserRole> UserRoles { get; set; }
    }
}