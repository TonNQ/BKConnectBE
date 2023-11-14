using BKConnectBE.Model.Dtos.FriendRequestManagement;

namespace BKConnectBE.Service.FriendRequests
{
    public interface IFriendRequestService
    {
        Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId);
        Task RemoveFriendRequestById(string senderId, string userId);
        Task<bool> CheckFriendRequest(string senderId, string receiverId);
        Task<bool> CheckCanSendFriendRequest(string senderId, string receiverId);
    }
}