using Microsoft.EntityFrameworkCore;
using SodalisDatabase.Entities;
using SodalisDatabase.Enums;

namespace SodalisDatabase {
    public class SodalisContext : DbContext {

        public SodalisContext(DbContextOptions<SodalisContext> options) : base(options) { }

        internal DbSet<User> Users { get; set; }
        internal DbSet<Goal> Goals { get; set; }
        internal DbSet<Friendship> Friendships { get ; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            //user table
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().HasAlternateKey(u => u.EmailAddress);
            builder.Entity<User>().HasIndex(u => u.EmailAddress);
            builder.Entity<User>().Property(u => u.IsDeleted).HasDefaultValue(false);

            //goal table
            builder.Entity<Goal>().HasKey(g => g.Id);
            builder.Entity<Goal>().HasIndex(g => g.UserId);
            builder.Entity<Goal>().Property(g => g.IsPublic).HasDefaultValue(false);
            builder.Entity<Goal>().Property(g => g.Status).HasConversion(
                status => (int) status, status => (GoalStatus) status);

            //friendship table
            builder.Entity<Friendship>().HasKey(f => new { f.SenderId, f.ReceiverId });
            builder.Entity<Friendship>().HasIndex(f => new { f.ReceiverId, f.SenderId });
            builder.Entity<Friendship>().Property(f => f.Accepted).HasDefaultValue(false);
        }
    }
}