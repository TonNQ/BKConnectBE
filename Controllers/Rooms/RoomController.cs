using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Common.Enumeration;
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

        [HttpGet("getListOfPublicRooms")]
        public async Task<ActionResult<Responses>> GetListOfPublicRooms()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.GetListOfRoomsByTypeAndUserId(nameof(RoomType.PublicRoom), userId);
                    return this.Success(listOfRooms, MsgNo.SUCCESS_GET_ROOMS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getListOfClassRooms")]
        public async Task<ActionResult<Responses>> GetListOfClassRooms()
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.GetListOfRoomsByTypeAndUserId(nameof(RoomType.ClassRoom), userId);
                    return this.Success(listOfRooms, MsgNo.SUCCESS_GET_ROOMS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("searchListOfPublicRooms")]
        public async Task<ActionResult<Responses>> SearchListOfPublicRooms([FromQuery] SearchKeyCondition condition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.SearchListOfRoomsByTypeAndUserId(nameof(RoomType.PublicRoom), userId, condition.SearchKey);
                    return this.Success(listOfRooms, MsgNo.SUCCESS_GET_ROOMS_OF_USER);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("searchListOfClassRooms")]
        public async Task<ActionResult<Responses>> SearchListOfClassRooms([FromQuery] SearchKeyCondition condition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.SearchListOfRoomsByTypeAndUserId(nameof(RoomType.ClassRoom), userId, condition.SearchKey);
                    return this.Success(listOfRooms, MsgNo.SUCCESS_GET_ROOMS_OF_USER);
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