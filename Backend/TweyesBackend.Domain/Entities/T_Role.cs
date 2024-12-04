using Microsoft.AspNetCore.Identity;

namespace TweyesBackend.Domain.Entities
{
    public class T_Role : IdentityRole
    {
        public virtual IList<T_UserRole>? UserRoles { get; set; }
    }
}