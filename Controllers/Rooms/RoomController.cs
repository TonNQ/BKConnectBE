using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
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
        public async Task<ActionResult<Responses>> SearchListOfRoomsByUserId(string searchKey)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listOfRooms = await _roomService.GetListOfRoomsByUserId(userId, searchKey);
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