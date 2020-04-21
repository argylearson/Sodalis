using System.Data;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SodalisDatabase.Entities;
using SodalisDatabase.Enums;

namespace SodalisDatabase.ContextExtensions {
    public static class GoalSodalisExtension {
        private static readonly string GetGoalsByUserIdSproc = "[dbo].[GetGoalsByUserId] @userId, @includePrivate, @pageNumber, @pageSize";

        public static Task<Goal> GetGoalById(this SodalisContext context, int id) {
            return context.Goals.FindAsync(id).AsTask();
        }

        public static Task<Goal[]> GetGoalsByUserId(this SodalisContext context, int userId, bool includePrivate,
            int pageNumber, int pageSize) {
            object[] parameters = {
                new SqlParameter {ParameterName = "userId", DbType = DbType.Int32, Direction = ParameterDirection.Input, Value = userId},
                new SqlParameter {ParameterName = "includePrivate", DbType = DbType.Boolean, Direction = ParameterDirection.Input, Value = includePrivate}, 
                new SqlParameter {ParameterName = "pageNumber", DbType = DbType.Int32, Direction = ParameterDirection.Input, Value = pageNumber},
                new SqlParameter {ParameterName = "pageSize", DbType = DbType.Int32, Direction = ParameterDirection.Input, Value = pageSize}
            };
            return context.Goals.FromSqlRaw(GetGoalsByUserIdSproc, parameters).ToArrayAsync();
        }

        public static async Task<Goal> CreateGoal(this SodalisContext context, Goal goal) {
            _ = await context.Goals.AddAsync(goal);
            _ =  await context.SaveChangesAsync();
            return goal;
        }

        public static async Task<Goal> UpdateGoal(this SodalisContext context, Goal goal) {
            var originalGoal = await context.Goals.FindAsync(goal.Id);
            originalGoal.Status = goal.Status == GoalStatus.NotProvided ? originalGoal.Status : goal.Status;
            originalGoal.Title = string.IsNullOrEmpty(goal.Title) ? originalGoal.Title : goal.Title;
            originalGoal.Description = string.IsNullOrEmpty(goal.Description) ? originalGoal.Description : goal.Description;
            originalGoal.IsPublic = goal.IsPublic;

            _ = await context.SaveChangesAsync();
            return originalGoal;
        }

        public static Task DeleteGoal(this SodalisContext context, Goal goal) {
            _ = context.Goals.Attach(goal);
            _ = context.Goals.Remove(goal);
            return context.SaveChangesAsync();
        }
    }
}