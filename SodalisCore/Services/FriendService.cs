using System.Threading.Tasks;
using SodalisDatabase.Entities;

namespace SodalisCore.Services {
    public class FriendService : IFriendService {
        Task IFriendService.DeleteFriendship(int userId, int friendId) {
            throw new System.NotImplementedException();
        }

        Task<Friendship[]> IFriendService.GetFriendsRequests(int userId, int pageNumber, int pageSize) {
            throw new System.NotImplementedException();
        }

        Task<Friendship> IFriendService.SendFriendRequest(int userId, int friendId) {
            throw new System.NotImplementedException();
        }
    }
}