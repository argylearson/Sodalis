using System.Threading.Tasks;
using SodalisDatabase.Entities;

namespace SodalisCore.Repositories {
    public interface IFriendRepository {
        internal Task<Friendship[]> GetFriendsRequests(int userId, int pageNumber, int pageSize);
        internal Task<Friendship> SendFriendRequest(int userId, int friendId);
        internal Task DeleteFriendship(int userId, int friendId);
    }
}