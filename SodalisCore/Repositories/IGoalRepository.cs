using System.Threading.Tasks;
using SodalisDatabase.Entities;

namespace SodalisCore.Repositories {
    public interface IGoalRepository {
        internal Task<Goal> GetGoalById(int goalId);
        internal Task<Goal[]> GetGoalsByUserId(int userId, bool includePrivate, int pageNumber, int pageSize);
        internal Task<Goal> CreateGoal(Goal goal);
        internal Task<Goal> UpdateGoal(Goal goal);
        internal Task DeleteGoal(Goal goal);
    }
}