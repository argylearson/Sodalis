using System.Threading.Tasks;
using SodalisDatabase.Entities;

namespace SodalisCore.Services {
    public interface IFriendService {
        internal Task<Friendship[]> GetFriendsRequests(int userId, int pageNumber = 1, int pageSize = 25);
        internal Task<Friendship> SendFriendRequest(int userId, int friendId);
        internal Task DeleteFriendship(int userId, int friendId);
    }
}