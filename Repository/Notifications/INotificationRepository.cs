using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Notifications
{
    public interface INotificationRepository
    {
        Task<List<Notification>> GetListOfNotificationsByUserIdAsync(string userId);
        Task RemoveFriendRequestNotificationAsync(string sender, string receiverId);
        Task UpdateNotificationsStatusOfUserAsync(string userId);
    }
}