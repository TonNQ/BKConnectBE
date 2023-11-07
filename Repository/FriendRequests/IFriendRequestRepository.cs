using BKConnectBE.Model.Dtos.FriendRequestManagement;

namespace BKConnectBE.Repository.FriendRequests
{
    public interface IFriendRequestRepository
    {
        Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId);
        Task<List<SentFriendRequestDto>> GetListOfAcceptedFriendRequestsOfUser(string userId);
        Task<ReceivedFriendRequestDto> GetFriendRequestByUser(string senderId, string receiverId);
        Task<bool> CheckFriendRequest(string senderId, string receiverId);
        Task<bool> CheckCanSendFriendRequest(string senderId, string receiverId);
        Task CreateFriendRequest(string senderId, string receiverId);
        Task UpdateStatusOfListFriendRequests(string usserId);
    }
}