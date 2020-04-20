using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SodalisCore.Services;
using SodalisDatabase.Entities;

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
        public async Task<IActionResult> GetGoalsForLoggedInUser() {
            async Task<IActionResult> Action() {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                var goals = await _goalService.GetGoalsByUserId(userId);
                return new ObjectResult(goals) {StatusCode = 200};
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpGet]
        [Route("user/{id}")]
        public async Task<IActionResult> GetGoalsForUser(int id) {
            async Task<IActionResult> Action() {
                var goals = await _goalService.GetGoalsByUserId(id);
                return new ObjectResult(goals) { StatusCode = 200 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpPost]
        public async Task<IActionResult> CreateGoal([FromBody] Goal goal) {
            async Task<IActionResult> Action() {
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
                var updatedGoal = await _goalService.UpdateGoal(id, goal);
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