using BKConnectBE.Model.Dtos.NotificationManagement;

namespace BKConnectBE.Service.Notifications
{
    public interface INotificationService
    {
        Task<List<ReceiveNotificationDto>> GetListOfNotificationsByUserIdAsync(string userId);
        Task UpdateNotificationsStatusOfUserAsync(string userId);
        Task<ReceiveNotificationDto> AddSendFriendRequestNotification(string senderId, string receiverId);
        Task<ReceiveNotificationDto> AddAcceptedFriendRequestNotification(string senderId, string receivedId);
        Task<ReceiveNotificationDto> AddRoomNotification(string senderId, string receiverId, string type, long roomId);
        Task<ReceiveNotificationDto> AddPostFileNotification(long fileId, string receiverId);
    }
}