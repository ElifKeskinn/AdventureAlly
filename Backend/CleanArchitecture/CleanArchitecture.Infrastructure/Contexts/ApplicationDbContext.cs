using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace CleanArchitecture.Infrastructure.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IDateTimeService _dateTime;
        private readonly IAuthenticatedUserService _authenticatedUser;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IDateTimeService dateTime, IAuthenticatedUserService authenticatedUser) : base(options)
        {
            ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            _dateTime = dateTime;
            _authenticatedUser = authenticatedUser;
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Deal> Deal { get; set; }
        public DbSet<DealValidity> DealValidity { get; set; }
        public DbSet<LocalBusiness> LocalBusiness { get; set; }
        public DbSet<NotificationPreferences> NotificationPreferences { get; set; }
        public DbSet<TourPackage> TourPackages { get; set; }
        public DbSet<UserPreferences> UserPreferences { get; set; }
        public DbSet<Interests> Interests { get; set; }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<AuditableBaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.Created = _dateTime.NowUtc;
                        entry.Entity.CreatedBy = _authenticatedUser.UserId;
                        break;
                    case EntityState.Modified:
                        entry.Entity.LastModified = _dateTime.NowUtc;
                        entry.Entity.LastModifiedBy = _authenticatedUser.UserId;
                        break;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<ApplicationUser>(entity =>
            {
                entity.ToTable(name: "Users");
            });


            builder.Entity<Deal>(entity =>
            {
                entity.ToTable(name: "Deals");
                entity.HasMany(d => d.DealValidities)
                      .WithOne(dv => dv.Deal)
                      .HasForeignKey(dv => dv.DealId);
            });

            builder.Entity<DealValidity>(entity =>
            {
                entity.ToTable(name: "DealValidities");
                entity.HasOne(dv => dv.Deal)
                      .WithMany(d => d.DealValidities)
                      .HasForeignKey(dv => dv.DealId);
            });

            builder.Entity<LocalBusiness>(entity =>
            {
                entity.ToTable(name: "LocalBusinesses");
            });

            builder.Entity<NotificationPreferences>(entity =>
            {
                entity.ToTable(name: "NotificationPreferences");
            });

            builder.Entity<TourPackage>(entity =>
            {
                entity.ToTable(name: "TourPackages");
            });

          builder.Entity<User>()
                   .HasOne(u => u.UserPreferences)
                   .WithOne(up => up.User)
                   .HasForeignKey<UserPreferences>(up => up.UserId);


            builder.Entity<IdentityRole>(entity =>
            {
                entity.ToTable(name: "Role");
            });
            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.ToTable("UserRoles");
            });

            builder.Entity<IdentityUserClaim<string>>(entity =>
            {
                entity.ToTable("UserClaims");
            });

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.ToTable("UserLogins");
            });

            builder.Entity<IdentityRoleClaim<string>>(entity =>
            {
                entity.ToTable("RoleClaims");

            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.ToTable("UserTokens");
            });

            //All Decimals will have 18,6 Range
            foreach (var property in builder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
            {
                property.SetColumnType("decimal(18,6)");
            }
            base.OnModelCreating(builder);
        }
    }
}
