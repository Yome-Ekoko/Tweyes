using TweyesBackend.Domain.Constant;
using TweyesBackend.Domain.Entities;

namespace TweyesBackend.Persistence.Seeds
{
    public static class DefaultRoles
    {
        public static List<T_Role> IdentityRoleList()
        {
            return new List<T_Role>()
            {
                new() {
                    Id = RoleConstants.Tutor,
                    Name = Roles.Tutor,
                    NormalizedName = Roles.Tutor.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a23"
                },
                new() {
                    Id = RoleConstants.Student,
                    Name = Roles.Student,
                    NormalizedName = Roles.Student.ToUpper(),
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147a56"
                }
            };
        }
    }
}
