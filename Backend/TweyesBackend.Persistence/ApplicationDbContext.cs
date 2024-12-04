using TweyesBackend.Domain.Entities;
using TweyesBackend.Domain.Entities.Base;
using TweyesBackend.Domain.Enum;
using TweyesBackend.Domain.Settings;
using TweyesBackend.Persistence.Seeds;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;
using System.Data;
using TweyesBackend.Domain.Common;

namespace TweyesBackend.Persistence
{
    public class ApplicationDbContext : IdentityDbContext<T_User, T_Role, string,
        IdentityUserClaim<string>, T_UserRole, IdentityUserLogin<string>,
        IdentityRoleClaim<string>, IdentityUserToken<string>>, IApplicationDbContext
    {
        private IDbContextTransaction? _currentTransaction;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly DatabaseOptions _databaseOptions;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            IHttpContextAccessor httpContextAccessor,
            IOptions<DatabaseOptions> databaseOptions) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _databaseOptions = databaseOptions.Value;
        }


        public DbSet<Tutor> Tutor { get; set; }
        public DbSet<Schedule> Schedules { get; set; }

   
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Remove default schema setting
            // modelBuilder.HasDefaultSchema("TweyesBackend");

            #region Identity Entities
            modelBuilder.Entity<T_User>(entity =>
            {
                entity.ToTable(name: "T_USER");

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");
                entity.Property(x => x.AccessFailedCount)
                    .HasColumnName("ACCESS_FAILED_COUNT");
                entity.Property(x => x.ConcurrencyStamp)
                    .HasColumnName("CONCURRENCY_STAMP");
                entity.Property(x => x.CreatedAt)
                    .HasColumnName("CREATED_AT");
                entity.Property(x => x.UpdatedAt)
                    .HasColumnName("UPDATED_AT");
                entity.Property(x => x.Email)
                    .HasColumnName("EMAIL")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.EmailConfirmed)
                    .HasColumnName("EMAIL_CONFIRMED");
                entity.Property(x => x.FirstName)
                    .HasColumnName("NAME")
                    .HasMaxLength(100); // Adjust column length
                entity.Property(x => x.Status)
                    .HasConversion(new EnumToStringConverter<UserStatus>())
                    .HasColumnName("STATUS");
                entity.Property(x => x.IsLoggedIn)
                    .HasColumnName("IS_LOGGED_IN");
                entity.Property(x => x.LastLoginTime)
                    .HasColumnName("LAST_LOGIN_TIME");
                entity.Property(x => x.LockoutEnabled)
                    .HasColumnName("LOCKOUT_ENABLED");
                entity.Property(x => x.LockoutEnd)
                    .HasColumnName("LOCKOUT_END");
                entity.Property(x => x.NormalizedEmail)
                    .HasColumnName("NORMALIZED_EMAIL")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.NormalizedUserName)
                    .HasColumnName("NORMALIZED_USER_NAME")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.PasswordHash)
                    .HasColumnName("PASSWORD_HASH")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.PhoneNumber)
                    .HasColumnName("PHONE_NUMBER")
                    .HasMaxLength(20); // Adjust column length
                entity.Property(x => x.PhoneNumberConfirmed)
                    .HasColumnName("PHONE_NUMBER_CONFIRMED");
                entity.Property(x => x.SecurityStamp)
                    .HasColumnName("SECURITY_STAMP");
                entity.Property(x => x.TwoFactorEnabled)
                    .HasColumnName("TWO_FACTOR_ENABLED");
                entity.Property(x => x.UserName)
                    .HasColumnName("USER_NAME")
                    .HasMaxLength(256); // Adjust column length


                // Each User can have many UserClaims
                entity.HasMany(e => e.Claims)
                    .WithOne()
                    .HasForeignKey(uc => uc.UserId);
                //.IsRequired();


                // Each User can have many UserLogins
                entity.HasMany(e => e.Logins)
                    .WithOne()
                    .HasForeignKey(ul => ul.UserId);
                    //.IsRequired();

                // Each User can have many UserTokens
                entity.HasMany(e => e.Tokens)
                    .WithOne()
                    .HasForeignKey(ut => ut.UserId);
                    //.IsRequired();

                entity.HasMany(e => e.UserRoles)
                    .WithOne(ur => ur.User)
                    .HasForeignKey(ur => ur.UserId);
                //.IsRequired();


                entity.HasOne(x => x.Tutor)
                    .WithOne(x => x.User)
                    .HasForeignKey<Tutor>(x => x.UserId);

            });
            #region Schedule Entity
            modelBuilder.Entity<Schedule>(entity =>
            {
                entity.ToTable("Schedules");

                entity.Property(x => x.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("ID");

                entity.Property(x => x.TutorId)
                    .IsRequired()
                    .HasMaxLength(256); 

                entity.Property(x => x.StudentId)
                    .IsRequired()
                    .HasMaxLength(256); 

                entity.Property(x => x.Day)
                    .IsRequired();

                entity.Property(x => x.Time)
                    .IsRequired();

                entity.Property(x => x.Time1)
                    .IsRequired();

                entity.Property(x => x.Status)
                    .IsRequired();

                entity.HasOne(x => x.Student)
                    .WithMany(x => x.Schedules)
                    .HasForeignKey(x => x.StudentId)
                    .IsRequired();
                
                entity.HasOne(x => x.Tutor)
                    .WithMany(x => x.Schedules)
                    .HasForeignKey(x => x.TutorId)
                    .IsRequired();
                //entity.HasOne(s => s.Tutor)
                //    .WithMany()
                //    .HasForeignKey(s => s.TutorId)
                //    .OnDelete(DeleteBehavior.Restrict);

                //entity.HasOne(s => s.Student)
                //    .WithMany()
                //    .HasForeignKey(s => s.StudentId)
                //    .OnDelete(DeleteBehavior.Restrict);
            });
            #endregion

            modelBuilder.Entity<T_Role>(entity =>
            {
                entity.ToTable(name: "T_ROLE");
                entity.HasKey(x => x.Id);
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.ConcurrencyStamp)
                    .HasColumnName("CONCURRENCY_STAMP");
                entity.Property(x => x.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.NormalizedName)
                    .HasColumnName("NORMALIZED_NAME")
                    .HasMaxLength(256); // Adjust column length
                entity.HasMany(e => e.UserRoles)
                    .WithOne(ur => ur.Role)
                    .HasForeignKey(ur => ur.RoleId);
                    //.IsRequired();
            });

            modelBuilder.Entity<T_UserRole>(entity =>
            {
                entity.HasKey(ur => new { ur.UserId, ur.RoleId });
                entity.ToTable("USER_ROLES");
                entity.Property(x => x.RoleId)
                    .HasColumnName("ROLE_ID")
                    .HasMaxLength(256);
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(256);
            });

            modelBuilder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("T_USER_CLAIMS");
                entity.Property(x => x.ClaimType)
                    .HasColumnName("CLAIM_TYPE")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.ClaimValue)
                    .HasColumnName("CLAIM_VALUE")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("T_ROLE_CLAIMS");
                entity.Property(x => x.ClaimType)
                    .HasColumnName("CLAIM_TYPE")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.ClaimValue)
                    .HasColumnName("CLAIM_VALUE")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.Id)
                    .HasColumnName("ID");
                entity.Property(x => x.RoleId)
                    .HasColumnName("ROLE_ID");
            });

            modelBuilder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("T_USER_LOGINS");
                entity.Property(x => x.LoginProvider)
                    .HasColumnName("LOGIN_PROVIDER")
                    .HasMaxLength(128); // Adjust column length
                entity.Property(x => x.ProviderDisplayName)
                    .HasColumnName("PROVIDER_DISPLAY_NAME")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.ProviderKey)
                    .HasColumnName("PROVIDER_KEY")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");
            });

            modelBuilder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("T_USER_TOKENS");
                entity.Property(x => x.LoginProvider)
                    .HasColumnName("LOGIN_PROVIDER")
                    .HasMaxLength(128); // Adjust column length
                entity.Property(x => x.Name)
                    .HasColumnName("NAME")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.Value)
                    .HasColumnName("VALUE")
                    .HasMaxLength(256); // Adjust column length
                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID");


            });
            #endregion Identity Entities

            modelBuilder.Entity<Tutor>(entity =>
            {
                entity.ToTable("TUTOR");

                entity.Property(x => x.Qualifications)
                    .HasConversion(new JsonValueConverter<List<string>>())
                    .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());

                entity.Property(x => x.Languages)
                    .HasConversion(new JsonValueConverter<List<string>>())
                    .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());

                entity.Property(x => x.AvailableDays)
                    .HasConversion(new JsonValueConverter<List<string>>())
                    .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());

                entity.Property(x => x.AvailableTime1)
                    .HasConversion(new JsonValueConverter<List<string>>())
                    .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());

                entity.Property(x => x.AvailableTime)
                    .HasConversion(new JsonValueConverter<List<string>>())
                    .Metadata.SetValueComparer(new JsonValueComparer<List<string>>());

                entity.Property(x => x.UserId)
                    .HasColumnName("USER_ID")
                    .HasMaxLength(256);

            }).Model.SetMaxIdentifierLength(30);

            #region Scaffolded Entities

            #endregion Scaffolded Entities

            modelBuilder.Seed();
        }

        public async override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            AddAuditMeta();
            return await base.SaveChangesAsync(cancellationToken);
        }

        public async Task BeginTransactionAsync(
            IsolationLevel isolationLevel,
            CancellationToken cancellationToken = default
        )
        {
            _currentTransaction ??= await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await SaveChangesAsync(cancellationToken);
                await _currentTransaction?.CommitAsync(cancellationToken)!;
            }
            catch
            {
                await RollbackTransactionAsync(cancellationToken);
                throw;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await _currentTransaction?.RollbackAsync(cancellationToken)!;
            }
            finally
            {
                _currentTransaction?.Dispose();
                _currentTransaction = null;
            }
        }

        public Task RetryOnExceptionAsync(Func<Task> operation)
        {
            return Database.CreateExecutionStrategy().ExecuteAsync(operation);
        }

        public Task<TResult> RetryOnExceptionAsync<TResult>(Func<Task<TResult>> operation)
        {
            return Database.CreateExecutionStrategy().ExecuteAsync(operation);
        }

        public Task ExecuteTransactionalAsync(Func<Task> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    await action();

                    await transaction.CommitAsync(cancellationToken);
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        public Task<T> ExecuteTransactionalAsync<T>(Func<Task<T>> action, CancellationToken cancellationToken = default)
        {
            var strategy = Database.CreateExecutionStrategy();
            return strategy.ExecuteAsync(async () =>
            {
                await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
                try
                {
                    var result = await action();

                    await transaction.CommitAsync(cancellationToken);

                    return result;
                }
                catch
                {
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        private void AddTimestamp()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is EntityBase<IComparable> && x.State == EntityState.Added);

            foreach (var entity in entities)
            {
                var now = DateTime.Now; // current datetime

                ((EntityBase<IComparable>)entity.Entity).CreatedAt = now;
            }
        }

        private void AddAuditMeta()
        {
            var entities = ChangeTracker.Entries()
                .Where(x => x.Entity is AuditableEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));

            foreach (var entity in entities)
            {
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "userId")?.Value ?? "internal";
                var now = DateTime.Now; // current datetime

                if (entity.State == EntityState.Added)
                {
                    ((AuditableEntity)entity.Entity).CreatedBy = userId;
                }
                if (entity.State == EntityState.Modified)
                {
                    ((AuditableEntity)entity.Entity).UpdatedAt = now;
                    ((AuditableEntity)entity.Entity).UpdatedBy = userId;
                }
            }
        }
    }
}