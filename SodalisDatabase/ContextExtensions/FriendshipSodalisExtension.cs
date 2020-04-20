using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SodalisDatabase.Entities;

namespace SodalisDatabase.ContextExtensions {
    public static class FriendshipSodalisExtension {
        private static readonly string GetFriendshipsByUserIdSproc = "[dbo].[GetFriendshipsByUserId] @userId, @pageNumber, @pageSize";

        public static Task<Friendship[]> GetFriendshipsByUserId(this SodalisContext context,
            int userId, int pageNumber, int pageSize) {
            object[] parameters = {
                new SqlParameter {ParameterName = "userId", DbType = DbType.Int32, Direction = ParameterDirection.Input, Value = userId},
                new SqlParameter {ParameterName = "pageNumber", DbType = DbType.Int32, Direction = ParameterDirection.Input, Value = pageNumber},
                new SqlParameter {ParameterName = "pageSize", DbType = DbType.Int32, Direction = ParameterDirection.Input, Value = pageSize}
            };
            return context.Friendships.FromSqlRaw(GetFriendshipsByUserIdSproc, parameters).ToArrayAsync();
        }

        public static Task<Friendship> GetFriendshipByUserIds(this SodalisContext context, int senderId, int receiverId) {
            return context.Friendships.FindAsync(senderId, receiverId).AsTask();
        }

        public static async Task<Friendship> RequestFriendship(this SodalisContext context, int senderId, int receiverId) {
            var friendship = new Friendship {
                SenderId = senderId,
                ReceiverId = receiverId
            };
            _ = await context.Friendships.AddAsync(friendship);
            _ = await context.SaveChangesAsync();
            return friendship;
        }

        public static async Task<Friendship> AcceptFriendship(this SodalisContext context, Friendship friendship) {
            _ = context.Friendships.Attach(friendship);
            friendship.Accepted = true;
            _ = await context.SaveChangesAsync();
            return friendship;
        }

        public static Task DeleteFriendship(this SodalisContext context, Friendship friendship) {
            _ = context.Friendships.Attach(friendship);
            _ = context.Friendships.Remove(friendship);
            return context.SaveChangesAsync();
        }
    }
}