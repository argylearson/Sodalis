using Microsoft.EntityFrameworkCore;
using SodalisDatabase.Entities;

namespace SodalisDatabase {
    public class SodalisContext : DbContext {

        internal DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder) {
            //user table
            builder.Entity<User>().HasKey(u => u.Id);
            builder.Entity<User>().HasAlternateKey(u => u.EmailAddress);
            builder.Entity<User>().HasIndex(u => u.EmailAddress);
        }
    }
}