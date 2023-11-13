using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.FriendRequestManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.FriendRequests;
using BKConnectBE.Repository.Relationships;
using BKConnectBE.Repository.Rooms;

namespace BKConnectBE.Service.FriendRequests
{
    public class FriendRequestService : IFriendRequestService
    {
        private readonly IGenericRepository<FriendRequest> _genericRepositoryForFriendRequest;
        private readonly IFriendRequestRepository _friendRequestRepository;
        private readonly IRoomRepository _roomRepository;
        private readonly IRelationshipRepository _relationshipRepository;

        public FriendRequestService(IGenericRepository<FriendRequest> genericRepositoryForFriendRequest,
            IFriendRequestRepository friendRequestRepository,
            IRoomRepository roomRepository,
            IRelationshipRepository relationshipRepository)
        {
            _genericRepositoryForFriendRequest = genericRepositoryForFriendRequest;
            _friendRequestRepository = friendRequestRepository;
            _roomRepository = roomRepository;
            _relationshipRepository = relationshipRepository;
        }

        public async Task<List<SentFriendRequestDto>> GetListOfAcceptedFriendRequestsOfUser(string userId)
        {
            return await _friendRequestRepository.GetListOfAcceptedFriendRequestsOfUser(userId);
        }

        public async Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId)
        {
            return await _friendRequestRepository.GetListOfReceivedFriendRequestsOfUser(userId);
        }

        public async Task RemoveFriendRequestById(long friendRequestId, string userId)
        {
            var friendRequest = await _genericRepositoryForFriendRequest.GetByIdAsync(friendRequestId)
                ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
            if (userId != friendRequest.SenderId && userId != friendRequest.ReceiverId)
            {
                throw new Exception(MsgNo.ERROR_REMOVE_FRIEND_REQUEST);
            }
            await _genericRepositoryForFriendRequest.RemoveByIdAsync(friendRequestId);
            await _genericRepositoryForFriendRequest.SaveChangeAsync();
        }

        public async Task<bool> CheckFriendRequest(string senderId, string receiverId)
        {
            return await _friendRequestRepository.CheckFriendRequest(senderId, receiverId);
        }

        public async Task<bool> CheckCanSendFriendRequest(string senderId, string receiverId)
        {
            return await _friendRequestRepository.CheckCanSendFriendRequest(senderId, receiverId);
        }

        public async Task<ReceivedFriendRequestDto> CreateFriendRequest(string senderId, string receiverId)
        {
            if (!await CheckCanSendFriendRequest(senderId, receiverId)) throw new Exception(MsgNo.ERROR_CREATE_FRIEND_REQUEST);
            await _friendRequestRepository.CreateFriendRequest(senderId, receiverId);
            await _genericRepositoryForFriendRequest.SaveChangeAsync();
            return await _friendRequestRepository.GetLastFriendRequestByUser(senderId, receiverId);
        }

        public async Task AcceptFriendRequest(long friendRequestId, string userId)
        {
            var friendRequest = await _genericRepositoryForFriendRequest.GetByIdAsync(friendRequestId)
                ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
            if (friendRequest.ReceiverId != userId || friendRequest.Status == FriendRequestStatus.Accepted.ToString())
            {
                throw new Exception(MsgNo.ERROR_RESPONSE_FRIEND_REQUEST);
            }

            friendRequest.Status = FriendRequestStatus.Accepted.ToString();
            await _roomRepository.CreateNewPrivateRoom(friendRequest.SenderId, friendRequest.ReceiverId, Constants.FRIEND_ACCEPTED_NOTIFICATION);
            await _relationshipRepository.CreateNewRelationship(friendRequest.SenderId, friendRequest.ReceiverId);
            await _genericRepositoryForFriendRequest.SaveChangeAsync();
        }

        public async Task UpdateStatusOfListFriendRequests(string userId)
        {
            await _friendRequestRepository.UpdateStatusOfListFriendRequests(userId);
            await _genericRepositoryForFriendRequest.SaveChangeAsync();
        }
    }
}