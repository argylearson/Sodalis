using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SodalisCore.Services;
using SodalisDatabase.Entities;
using SodalisExceptions;
using SodalisExceptions.Exceptions;

namespace SodalisCore.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GoalsController : BaseSodalisController
    {
        private readonly IGoalService _goalService;

        public GoalsController(IGoalService goalService) {
            _goalService = goalService;
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetGoalById(int id) {
            async Task<IActionResult> Action() {
                var goal = await _goalService.GetGoalById(id);
                return new ObjectResult(goal) { StatusCode = 200 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpGet]
        public async Task<IActionResult> GetGoalsForLoggedInUser([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null) {
            async Task<IActionResult> Action() {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                Goal[] goals;
                if (ParsePagingParameters(pageNumber, pageSize, out var pNumber, out var pSize))
                    goals = await _goalService.GetGoalsByUserId(userId, pNumber, pSize);
                else
                    goals = await _goalService.GetGoalsByUserId(userId);
                return new ObjectResult(goals) {StatusCode = 200};
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetGoalsForUser(int id, [FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null) {
            async Task<IActionResult> Action() {
                Goal[] goals;
                if (ParsePagingParameters(pageNumber, pageSize, out var pNumber, out var pSize))
                    goals = await _goalService.GetGoalsByUserId(id, pNumber, pSize);
                else
                    goals = await _goalService.GetGoalsByUserId(id);
                return new ObjectResult(goals) { StatusCode = 200 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] Goal goal) {
            async Task<IActionResult> Action() {
                if (goal.UserId != 0 && goal.UserId != int.Parse(HttpContext.User.Identity.Name))
                    throw new BadRequestException("User id's did not match") {
                        ClientMessage = new ErrorMessage("User id in request much be empty, zero, or your id.")
                    };
                if (goal.Id != 0) {
                    throw new BadRequestException("User provided an id when creating a goal") {
                        ClientMessage = new ErrorMessage("Do not provide a goal id when creating a goal")
                    };
                }
                var goals = await _goalService.CreateGoal(goal);
                return new ObjectResult(goals) { StatusCode = 201 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdateGoal(int id, [FromBody] Goal goal) {
            async Task<IActionResult> Action() {
                if (goal.Id != 0 && goal.Id != id)
                    throw new BadRequestException("Goal id's did not match") {
                        ClientMessage = new ErrorMessage("Goal id in request much be empty, zero, or match the id in the uri.")
                    };
                if (goal.UserId != 0 && goal.UserId != int.Parse(HttpContext.User.Identity.Name))
                    throw new BadRequestException("User id's did not match") {
                        ClientMessage = new ErrorMessage("User id in request much be empty, zero, or your id.")
                    };
                var updatedGoal = await _goalService.UpdateGoal(goal);
                return new ObjectResult(updatedGoal) { StatusCode = 200 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteGoal(int id) {
            async Task<IActionResult> Action() {
                await _goalService.DeleteGoal(id);
                return new ObjectResult("") { StatusCode = 204 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }
    }
}