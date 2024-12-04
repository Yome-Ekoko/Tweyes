using TweyesBackend.Domain.Enum;
using Microsoft.AspNetCore.Identity;

namespace TweyesBackend.Domain.Entities
{
    public class T_User : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string User { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string ContactAddress { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public bool IsActive { get; set; } = true;
        public string ImageUrl { get; set; } = string.Empty;
        public PoolRole PoolRole { get; set; } 
        public UserStatus Status { get; set; }
        public bool IsLoggedIn { get; set; }
        public DateTime LastLoginTime { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public Tutor? Tutor { get; set; }
        public string? TutorId { get; set; } = string.Empty;
        public virtual ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public virtual ICollection<T_UserRole> UserRoles { get; set; } = new List<T_UserRole>();
        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; } = new List<IdentityUserClaim<string>>();
        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; } = new List<IdentityUserLogin<string>>();
        public virtual ICollection<IdentityUserToken<string>> Tokens { get; set; } = new List<IdentityUserToken<string>>();
    }
}