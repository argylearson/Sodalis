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
    public class FriendsController : BaseSodalisController {
        private readonly IFriendService _friendService;

        public FriendsController(IFriendService friendService) {
            _friendService = friendService;
        }

        [HttpGet]
        public async Task<IActionResult> GetGoalsForLoggedInUser([FromQuery] int? pageNumber = null, [FromQuery] int? pageSize = null) {
            async Task<IActionResult> Action() {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                Friendship[] friendships;
                if (ParsePagingParameters(pageNumber, pageSize, out var pNumber, out var pSize)) {
                    friendships = await _friendService.GetFriendsRequests(userId, pNumber, pSize);
                }
                else
                    friendships = await _friendService.GetFriendsRequests(userId);
                return new ObjectResult(friendships) { StatusCode = 200 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpPost]
        [Route("{id}")]
        public async Task<IActionResult> SendFriendRequest(int id) {
            async Task<IActionResult> Action() {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                var friendship = await _friendService.SendFriendRequest(userId, id);
                return new ObjectResult(friendship) { StatusCode = 201 };
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteFriendRequest(int id) {
            async Task<IActionResult> Action() {
                var userId = int.Parse(HttpContext.User.Identity.Name);
                await _friendService.DeleteFriendship(userId, id);
                return new ObjectResult("") {StatusCode = 204};
            }

            var result = await ResultToResponseAsync(Action);
            return result;
        }
    }
}