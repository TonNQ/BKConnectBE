using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.FriendRequests;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.FriendRequests
{
    [CustomAuthorize]
    [ApiController]
    [Route("friendRequests")]
    public class FriendRequestController : ControllerBase
    {
        private readonly IFriendRequestService _friendRequestService;
        public FriendRequestController(IFriendRequestService friendRequestService)
        {
            _friendRequestService = friendRequestService;
        }

        [HttpGet("getListOfRecievedFriendRequests")]
        public async Task<ActionResult<Responses>> GetListOfRecievedFriendRequests()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var list = await _friendRequestService.GetListOfReceivedFriendRequestsOfUser(userId);
                    return this.Success(list, MsgNo.SUCCESS_GET_FRIEND_REQUESTS);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getListOfAcceptedFriendRequests")]
        public async Task<ActionResult<Responses>> GetListOfAcceptedFriendRequests()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var list = await _friendRequestService.GetListOfAcceptedFriendRequestsOfUser(userId);
                    return this.Success(list, MsgNo.SUCCESS_GET_FRIEND_REQUESTS);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("checkCanSendFriendRequest")]
        public async Task<ActionResult<Responses>> CheckCanSendFriendRequest([FromQuery] SearchKeyCondition condition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var data = new
                    {
                        result = await _friendRequestService.CheckCanSendFriendRequest(userId, condition.SearchKey)
                    };
                    return this.Success(data, MsgNo.SUCCESS_CHECK_FRIEND_REQUEST);
                }
                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpDelete("removeFriendRequest")]
        public async Task<ActionResult<Responses>> RemoveFriendRequest(LongKeyCondition friendRequest)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    await _friendRequestService.RemoveFriendRequestById(friendRequest.SearchKey, userId);
                    return this.Success(null, MsgNo.SUCCESS_REMOVE_FRIEND_REQUEST);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPost("acceptFriendRequest")]
        public async Task<ActionResult<Responses>> AcceptFriendRequest(LongKeyCondition friendRequest)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    await _friendRequestService.AcceptFriendRequest(friendRequest.SearchKey, userId);
                    return this.Success(null, MsgNo.SUCCESS_RESPONSE_FRIEND_REQUEST);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPut("updateStatusOfFriendRequests")]
        public async Task<ActionResult<Responses>> UpdateStatusOfListFriendRequests()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    await _friendRequestService.UpdateStatusOfListFriendRequests(userId);
                    return this.Success(null, MsgNo.SUCCESS_UPDATE_FRIEND_REQUEST);
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