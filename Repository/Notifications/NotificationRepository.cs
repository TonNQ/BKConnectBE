using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Notifications
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly BKConnectContext _context;

        public NotificationRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task<List<Notification>> GetListOfNotificationsByUserIdAsync(string userId)
        {
            return await _context.Notifications
                .Where(n => n.ReceiverId == userId)
                .OrderByDescending(n => n.SendTime)
                .ToListAsync();
        }

        public async Task UpdateNotificationsStatusOfUserAsync(string userId)
        {
            var notifications = await _context.Notifications
                .Where(n => n.ReceiverId == userId && !n.IsRead)
                .ToListAsync();
            notifications.ForEach(n => n.IsRead = true);
        }

        public async Task RemoveFriendRequestNotificationAsync(string senderId, string receiverId)
        {
            var list = new List<string> { senderId, receiverId };
            var notification = await _context.Notifications
                .FirstOrDefaultAsync(n => n.Type == NotificationType.IsSendFriendRequest.ToString()
                    && list.Contains(n.ReceiverId) && list.Contains(n.Content))
                    ?? throw new Exception(MsgNo.ERROR_NOTIFICATION_NOT_FOUND);
            _context.Notifications.Remove(notification);
        }
    }
}