using System.Threading.Tasks;
using SodalisCore.Repositories;
using SodalisDatabase.Entities;
using SodalisExceptions;
using SodalisExceptions.Exceptions;

namespace SodalisCore.Services {
    public class FriendService : IFriendService {
        private readonly IFriendRepository _friendRepository;

        public FriendService(IFriendRepository friendRepository) {
            _friendRepository = friendRepository;
        }

        Task<Friendship> IFriendService.GetFriendRequest(int userId, int friendId) {
            return _friendRepository.GetFriendRequest(userId, friendId);
        }

        Task<Friendship[]> IFriendService.GetFriendsRequests(int userId, int pageNumber, int pageSize) {
            return _friendRepository.GetFriendsRequests(userId, pageNumber, pageSize);
        }

        Task<Friendship> IFriendService.SendFriendRequest(int userId, int friendId) {
            if (friendId < 1)
                throw new BadRequestException("An invalid friend id was provided") {
                    ClientMessage = new ErrorMessage("Friend id must be positive. Please provide a valid Id and try again.")
                };

            return _friendRepository.SendFriendRequest(userId, friendId);
        }

        Task IFriendService.DeleteFriendship(int userId, int friendId) {
            if (friendId < 1)
                throw new BadRequestException("An invalid friend id was provided") {
                    ClientMessage = new ErrorMessage("Friend id must be positive. Please provide a valid Id and try again.")
                };
            return _friendRepository.DeleteFriendship(userId, friendId);
        }
    }
}