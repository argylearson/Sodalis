using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SodalisCore.Repositories;
using SodalisDatabase.Entities;
using SodalisDatabase.Enums;
using SodalisExceptions;
using SodalisExceptions.Exceptions;

namespace SodalisCore.Services {
    public class GoalService : IGoalService {
        private readonly IGoalRepository _goalRepository;
        private readonly IFriendService _friendService;
        private readonly IHttpContextAccessor _httpContext;

        public GoalService(IGoalRepository goalRepository, IFriendService friendService, IHttpContextAccessor contextAccessor) {
            _goalRepository = goalRepository;
            _friendService = friendService;
            _httpContext = contextAccessor;
        }

        async Task<Goal> IGoalService.GetGoalById(int goalId) {
            if (goalId < 1)
                throw new BadRequestException("Goal id was not positive") {
                    ClientMessage = new ErrorMessage("Goal id must be positive. Please provide a valid value and try again.")
                };
            try {
                var goal = await _goalRepository.GetGoalById(goalId);
                if (goal == null)
                    throw new NotFoundException($"No goal found for id {goalId}");

                var userId = int.Parse(_httpContext.HttpContext.User.Identity.Name);
                if (goal.UserId == userId)
                    return goal;

                if (!goal.IsPublic)
                    throw new NotFoundException("User requested goals for invalid user");

                var friendship = await _friendService.GetFriendRequest(userId, goal.UserId);
                if (friendship == null || !friendship.Accepted)
                    throw new NotFoundException("User requested goals for invalid user");

                return goal;
            } catch (NotFoundException ex) {
                ex.ClientMessage = new ErrorMessage($"No object found for id {goalId}");
                throw;
            }
        }

        async Task<Goal[]> IGoalService.GetGoalsByUserId(int userId, int pageNumber, int pageSize) {
            var currentUserId = int.Parse(_httpContext.HttpContext.User.Identity.Name);
            if (userId == currentUserId) 
                return await _goalRepository.GetGoalsByUserId(userId, true, pageNumber, pageSize);

            var friendship = await _friendService.GetFriendRequest(currentUserId, userId);
            if (friendship == null || !friendship.Accepted)
                throw new NotFoundException("User requested goals for invalid user") {
                    ClientMessage = new ErrorMessage("No goals were found for the provided user id.")
                };
            return await _goalRepository.GetGoalsByUserId(userId, false, pageNumber, pageSize);
        }

        Task<Goal> IGoalService.CreateGoal(Goal goal) {
            goal.UserId = int.Parse(_httpContext.HttpContext.User.Identity.Name);
            if (goal.Status == GoalStatus.NotProvided)
                goal.Status = GoalStatus.New;
            if (string.IsNullOrWhiteSpace(goal.Title))
                throw new BadRequestException("Goal does not have a title.") {
                    ClientMessage = new ErrorMessage("Title is required when creating a goal. Please provide a title and try again.")
                };
            return _goalRepository.CreateGoal(goal);
        }

        async Task<Goal> IGoalService.UpdateGoal(Goal goal) {
            if (goal.Id < 1)
                throw new BadRequestException("Goal id was not positive") {
                    ClientMessage = new ErrorMessage("Goal id must be positive. Please provide a valid value and try again.")
                };
            var originalGoal = await _goalRepository.GetGoalById(goal.Id);
            if (originalGoal == null || originalGoal.UserId != goal.UserId)
                throw new NotFoundException("User tried to update invalid goal.") {
                    ClientMessage = new ErrorMessage("No goal was found for the provided goal id")
                };
            return await _goalRepository.UpdateGoal(goal);
        }

        async Task IGoalService.DeleteGoal(int id) {
            var goal = await  _goalRepository.GetGoalById(id);
            var currentUserId = int.Parse(_httpContext.HttpContext.User.Identity.Name);
            if (goal == null || goal.UserId != currentUserId)
                throw new NotFoundException("User attempted to delete goal not belonging to them") {
                    ClientMessage = new ErrorMessage("No goal was found for the provided goal id.")
                };
            await _goalRepository.DeleteGoal(goal);
        }
    }
}