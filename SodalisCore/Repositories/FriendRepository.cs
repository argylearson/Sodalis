using SodalisDatabase.Entities;
using System.Threading.Tasks;
using SodalisDatabase;
using SodalisDatabase.ContextExtensions;
using SodalisExceptions;
using SodalisExceptions.Exceptions;

namespace SodalisCore.Repositories {
    public class FriendRepository : IFriendRepository {
        private readonly SodalisContext _sodalisContext;

        public FriendRepository(SodalisContext sodalisContext) {
            _sodalisContext = sodalisContext;
        }

        async Task<Friendship> IFriendRepository.GetFriendRequest(int userId, int friendId) {
            return await _sodalisContext.GetFriendshipByUserIds(userId, friendId) ??
                   await _sodalisContext.GetFriendshipByUserIds(friendId, userId);
        }

        Task<Friendship[]> IFriendRepository.GetFriendsRequests(int userId, int pageNumber, int pageSize) {
            return _sodalisContext.GetFriendshipsByUserId(userId, pageNumber, pageSize);
        }

        async Task<Friendship> IFriendRepository.SendFriendRequest(int userId, int friendId) {
            var friendShip = await _sodalisContext.GetFriendshipByUserIds(userId, friendId);
            if (friendShip != null)
                return friendShip;
            friendShip = await _sodalisContext.GetFriendshipByUserIds(friendId, userId);
            if (friendShip == null)
                return await _sodalisContext.RequestFriendship(userId, friendId);
            if (!friendShip.Accepted)
                return await _sodalisContext.AcceptFriendship(friendShip);
            return friendShip;

        }

        async Task IFriendRepository.DeleteFriendship(int userId, int friendId) {
            var friendShip = await _sodalisContext.GetFriendshipByUserIds(userId, friendId) ?? 
                             await _sodalisContext.GetFriendshipByUserIds(friendId, userId);
            if (friendShip == null)
                throw new NotFoundException("Friendship could not be deleted because no friendship was found.") {
                    ClientMessage = new ErrorMessage("No friendship was found between the two users.")
                };
            await _sodalisContext.DeleteFriendship(friendShip);
        }
    }
}