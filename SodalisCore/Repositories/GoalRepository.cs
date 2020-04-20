using SodalisDatabase.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SodalisDatabase;
using SodalisDatabase.ContextExtensions;

namespace SodalisCore.Repositories {
    public class GoalRepository : IGoalRepository {
        private readonly SodalisContext _sodalisContext;

        public GoalRepository(SodalisContext sodalisContext) {
            _sodalisContext = sodalisContext;
        }

        Task<Goal> IGoalRepository.GetGoalById(int goalId) {
            return _sodalisContext.GetGoalById(goalId);
        }

        Task<Goal[]> IGoalRepository.GetGoalsByUserId(int userId, bool includePrivate, int pageNumber, int pageSize) {
            return _sodalisContext.GetGoalsByUserId(userId, includePrivate, pageNumber, pageSize);
        }

        Task<Goal> IGoalRepository.CreateGoal(Goal goal) {
            return _sodalisContext.CreateGoal(goal);
        }

        Task<Goal> IGoalRepository.UpdateGoal(Goal goal) {
            return _sodalisContext.UpdateGoal(goal);
        }

        Task IGoalRepository.DeleteGoal(Goal goal) {
            return _sodalisContext.DeleteGoal(goal);
        }
    }
}