using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Rooms
{
    [CustomAuthorize]
    [ApiController]
    [Route("rooms")]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        [HttpGet("getRoomsOfUser")]
        public async Task<ActionResult<Responses>> GetListOfRoomsByUserId()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.GetListOfRoomsByUserId(userId);
                    return this.Success(listOfRooms, MsgNo.SUCCESS_GET_ROOMS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("searchRoomsOfUser")]
        public async Task<ActionResult<Responses>> SearchListOfRoomsByUserId([FromQuery] SearchKeyCondition searchKeyCondition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.GetListOfRoomsByUserId(userId, searchKeyCondition.SearchKey);
                    return this.Success(listOfRooms, MsgNo.SUCCESS_GET_ROOMS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getInformationOfRoom")]
        public async Task<ActionResult<Responses>> GetInformationOfRoom([FromQuery] LongKeyCondition condition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var room = await _roomService.GetInformationOfRoom(condition.SearchKey, userId);
                    return this.Success(room, MsgNo.SUCCESS_GET_INFORMATION_OF_ROOM);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getListOfMembersInRoom")]
        public async Task<ActionResult<Responses>> GetListOfMembersInRoom([FromQuery] LongKeyCondition condition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfMembers = await _roomService.GetListOfMembersInRoomAsync(condition.SearchKey, userId);
                    return this.Success(listOfMembers, MsgNo.SUCCESS_GET_LIST_OF_MEMBERS_IN_ROOM);
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