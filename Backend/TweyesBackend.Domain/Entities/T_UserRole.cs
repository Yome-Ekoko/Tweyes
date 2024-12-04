using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace TweyesBackend.Domain.Entities
{
    public class T_UserRole : IdentityUserRole<string>
    {
        [StringLength(450)]
        public override string UserId { get; set; } = string.Empty;

        public virtual T_User? User { get; set; }
        [StringLength(450)]

        public override string RoleId { get; set; } = string.Empty;

        public virtual T_Role? Role { get; set; }
    }
}