using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.Messages;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Messages
{
    [CustomAuthorize]
    [ApiController]
    [Route("messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageSerive;
        public MessageController(IMessageService messageService)
        {
            _messageSerive = messageService;
        }

        [HttpGet("getAllMessages")]
        public async Task<ActionResult<Responses>> GetAllMessagesInRoom([FromQuery] LongKeyCondition longKeyCondition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listMessages = await _messageSerive.GetAllMessagesInRoomAsync(userId, longKeyCondition.SearchKey);
                    return this.Success(listMessages, MsgNo.SUCCESS_GET_LIST_MESSAGES);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getAllImageMessages")]
        public async Task<ActionResult<Responses>> GetAllImageMessagesInRoom([FromQuery] LongKeyCondition longKeyCondition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listMessages = await _messageSerive.GetAllNoneTextMessagesInRoomAsync(longKeyCondition.SearchKey, MessageType.Image.ToString(), userId);
                    return this.Success(listMessages, MsgNo.SUCCESS_GET_LIST_MESSAGES);
                }
                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpGet("getAllFileMessages")]
        public async Task<ActionResult<Responses>> GetAllFileMessagesInRoom([FromQuery] LongKeyCondition longKeyCondition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listMessages = await _messageSerive.GetAllNoneTextMessagesInRoomAsync(longKeyCondition.SearchKey, MessageType.File.ToString(), userId);
                    return this.Success(listMessages, MsgNo.SUCCESS_GET_LIST_MESSAGES);
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