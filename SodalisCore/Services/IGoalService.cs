using System.Threading.Tasks;
using SodalisDatabase.Entities;

namespace SodalisCore.Services {
    public interface IGoalService {
        internal Task<Goal> GetGoalById(int goalId);
        internal Task<Goal[]> GetGoalsByUserId(int userId, int pageNumber = 1, int pageSize = 25);
        internal Task<Goal> CreateGoal(Goal goal);
        internal Task<Goal> UpdateGoal(Goal goal);
        internal Task DeleteGoal(int id);
    }
}