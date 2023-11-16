using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Model.Dtos.FriendRequestManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.FriendRequests;
using BKConnectBE.Repository.Notifications;
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
        private readonly INotificationRepository _notificationRepository;

        public FriendRequestService(IGenericRepository<FriendRequest> genericRepositoryForFriendRequest,
            IFriendRequestRepository friendRequestRepository,
            IRoomRepository roomRepository,
            IRelationshipRepository relationshipRepository,
            INotificationRepository notificationRepository)
        {
            _genericRepositoryForFriendRequest = genericRepositoryForFriendRequest;
            _friendRequestRepository = friendRequestRepository;
            _roomRepository = roomRepository;
            _relationshipRepository = relationshipRepository;
            _notificationRepository = notificationRepository;
        }

        public async Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId)
        {
            return await _friendRequestRepository.GetListOfReceivedFriendRequestsOfUser(userId);
        }

        public async Task<List<ReceivedFriendRequestDto>> SearchListOfReceivedFriendRequestsOfUser(string userId, string searchKey = "")
        {
            searchKey = Helper.RemoveUnicodeSymbol(searchKey);
            var list = await _friendRequestRepository.GetListOfReceivedFriendRequestsOfUser(userId);
            return list.Where(l => Helper.RemoveUnicodeSymbol(l.SenderName).Contains(searchKey)).ToList();
        }

        public async Task RemoveFriendRequestById(string senderId, string receiverId)
        {
            var friendRequest = await _friendRequestRepository.GetFriendRequestByUser(senderId, receiverId)
                ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);

            await _notificationRepository.RemoveFriendRequestNotificationAsync(friendRequest.SenderId, friendRequest.ReceiverId);
            _genericRepositoryForFriendRequest.Remove(friendRequest);
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
    }
}