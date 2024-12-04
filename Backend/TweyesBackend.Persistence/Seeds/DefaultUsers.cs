using TweyesBackend.Domain.Constant;
using TweyesBackend.Domain.Entities;
using TweyesBackend.Domain.Enum;

namespace TweyesBackend.Persistence.Seeds
{
    public static class DefaultUsers
    {
        public static List<T_User> UserList()
        {
            return new List<T_User>()
            {
                new() {
                    Id = RoleConstants.TutorUser,
                    UserName = "yummy",
                    Email = "yomeekoko25@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "yomeekoko25@gmail.com",
                    NormalizedUserName="YUMMY",
                    FirstName = "Yome Ekoko",
                    Status = UserStatus.Active,
                    PoolRole = PoolRole.Tutor,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e45",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e93",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2023-10-20"),
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                },
                new() {
                    Id = RoleConstants.StudentUser,
                    UserName = "ebube",
                    Email = "yomeekoko25@gmail.com",
                    EmailConfirmed = true,
                    PhoneNumberConfirmed = true,
                    // Password@123
                    PasswordHash = "AQAAAAEAACcQAAAAEBLjouNqaeiVWbN0TbXUS3+ChW3d7aQIk/BQEkWBxlrdRRngp14b0BIH0Rp65qD6mA==",
                    NormalizedEmail= "yomeekoko25@gmail.com",
                    NormalizedUserName="EBUBE",
                    FirstName = "Ebube Tutor",
                    Status = UserStatus.Active,
                     PoolRole = PoolRole.Tutor,
                    IsLoggedIn = false,
                    ConcurrencyStamp = "71f781f7-e957-469b-96df-9f2035147e98",
                    SecurityStamp = "71f781f7-e957-469b-96df-9f2035147e37",
                    AccessFailedCount = 0,
                    LockoutEnabled = false,
                    LastLoginTime = DateTime.Parse("2023-10-20"),
                    CreatedAt = DateTime.Parse("2023-10-20"),
                    UpdatedAt = DateTime.Parse("2023-10-20")
                }
            };
        }
    }
}