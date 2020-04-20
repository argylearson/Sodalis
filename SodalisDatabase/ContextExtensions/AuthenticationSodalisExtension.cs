using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SodalisDatabase.Entities;

namespace SodalisDatabase.ContextExtensions {
    public static class AuthenticationSodalisExtension {
        public static async Task<User> CreateUser(this SodalisContext context, User user) {
            _ = await context.Users.AddAsync(user);
            _ = await context.SaveChangesAsync();
            return user;
        }

        public static Task<User> GetUserByEmailAddress(this SodalisContext context, string emailAddress) {
            return context.Users.SingleOrDefaultAsync(u => string.Equals(u.EmailAddress, emailAddress, StringComparison.OrdinalIgnoreCase));
        }
    }
}