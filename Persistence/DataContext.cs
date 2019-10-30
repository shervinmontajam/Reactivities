using Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class DataContext : IdentityDbContext<AppUser>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Value> Values { get; set; }
        public DbSet<Activity> Activities { get; set; }
        public DbSet<UserActivity> UserActivities { get; set; }
        public DbSet<Photo> Photos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Value>().HasData(
                new Value { Id = 1, Name = "101" },
                new Value { Id = 2, Name = "102" },
                new Value { Id = 3, Name = "103" }
            );

            modelBuilder.Entity<UserActivity>(p => p.HasKey(ua => new
            {
                ua.AppUserId, ua.ActivityId
            }));

            modelBuilder.Entity<UserActivity>().HasOne(p => p.AppUser).WithMany(p => p.UserActivities).HasForeignKey(p => p.AppUserId);
            modelBuilder.Entity<UserActivity>().HasOne(p => p.Activity).WithMany(p => p.UserActivities).HasForeignKey(p => p.ActivityId);
        }
    }
}
