using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Service.BackgroundJobs;
using Hangfire;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.BackgroundJobs
{
    [CustomAuthorize]
    [ApiController]
    [Route("backgroundjobs")]
    public class BackgroundJobController : ControllerBase
    {
        private readonly IBackgroundJobService _backgroundJobService;

        public BackgroundJobController(IBackgroundJobService backgroundJobService)
        {
            _backgroundJobService = backgroundJobService;
        }

        [HttpPut("set-read-message-of-room")]
        public ActionResult<Responses> SetReadMessageOfRoom(ReadMessageOfRoomDto readMessage)
        {
            try
            {

                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    BackgroundJob.Enqueue(() => _backgroundJobService.SetReadMessageOfRoom(userId, readMessage));
                    return this.Success(null, MsgNo.SUCCESS_SET_READ_MESSAGE);
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