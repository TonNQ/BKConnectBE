using BKConnectBE.Model.Dtos.FriendRequestManagement;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.FriendRequests
{
    public interface IFriendRequestRepository
    {
        Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId);
        Task<FriendRequest> GetFriendRequestByUser(string user1Id, string user2Id);
        Task<bool> CheckFriendRequest(string senderId, string receiverId);
        Task<bool> CheckCanSendFriendRequest(string senderId, string receiverId);
        Task CreateFriendRequest(string senderId, string receiverId);
    }
}