using BKConnectBE.Model.Dtos.FriendRequestManagement;

namespace BKConnectBE.Service.FriendRequests
{
    public interface IFriendRequestService
    {
        Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId);
        Task<List<SentFriendRequestDto>> GetListOfAcceptedFriendRequestsOfUser(string userId);
        Task RemoveFriendRequestById(long friendRequestId, string userId);
        Task<bool> CheckFriendRequest(string senderId, string receiverId);
        Task<bool> CheckCanSendFriendRequest(string senderId, string receiverId);
        Task<ReceivedFriendRequestDto> CreateFriendRequest(string senderId, string receiverId);
        Task AcceptFriendRequest(long friendRequestId, string userId);
        Task UpdateStatusOfListFriendRequests(string userId);
    }
}