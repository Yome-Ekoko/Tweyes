using TweyesBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TweyesBackend.Persistence.Seeds
{
    public static class ContextSeed
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            CreateRoles(modelBuilder);

            CreateJwtUsers(modelBuilder);

            MapUserRole(modelBuilder);
        }

        private static void CreateRoles(ModelBuilder modelBuilder)
        {
            List<T_Role> roles = DefaultRoles.IdentityRoleList();
            modelBuilder.Entity<T_Role>().HasData(roles);
        }

        private static void CreateJwtUsers(ModelBuilder modelBuilder)
        {
            List<T_User> users = DefaultUsers.UserList();
            modelBuilder.Entity<T_User>().HasData(users);
        }

        private static void MapUserRole(ModelBuilder modelBuilder)
        {
            var identityUserRoles = MappingUserRole.IdentityUserRoleList();
            modelBuilder.Entity<T_UserRole>().HasData(identityUserRoles);
        }
    }
}
