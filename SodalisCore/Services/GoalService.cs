using System.Threading.Tasks;
using SodalisDatabase.Entities;

namespace SodalisCore.Services {
    public class GoalService : IGoalService {

        Task<Goal> IGoalService.GetGoalById(int goalId) {
            throw new System.NotImplementedException();
        }

        Task<Goal[]> IGoalService.GetGoalsByUserId(int userId) {
            throw new System.NotImplementedException();
        }

        Task<Goal> IGoalService.CreateGoal(Goal goal) {
            throw new System.NotImplementedException();
        }

        Task IGoalService.DeleteGoal(int id) {
            throw new System.NotImplementedException();
        }

        Task<Goal> IGoalService.UpdateGoal(int id, Goal goal) {
            throw new System.NotImplementedException();
        }
    }
}
}