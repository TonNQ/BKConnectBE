using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Service.Notifications;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Notifications
{
    [CustomAuthorize]
    [ApiController]
    [Route("notifications")]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("getListOfNotifications")]
        public async Task<ActionResult<Responses>> GetListOfNotifications()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var notifications = await _notificationService.GetListOfNotificationsByUserIdAsync(userId);
                    return this.Success(notifications, MsgNo.SUCCESS_GET_LIST_OF_NOTIFICATIONS);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPut("updateNotifications")]
        public async Task<ActionResult<Responses>> UpdateNotifications()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    await _notificationService.UpdateNotificationsStatusOfUserAsync(userId);
                    return this.Success(null, MsgNo.SUCCESS_UPDATE_NOTIFICATION_STATUS);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }
    }
}