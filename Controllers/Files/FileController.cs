using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.ClassFileManagement;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.Files;
using BKConnectBE.Service.WebSocket;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Files
{
    [CustomAuthorize]
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        private readonly IWebSocketService _webSocketService;
        public FileController(IFileService fileService,
            IWebSocketService webSocketService)
        {
            _fileService = fileService;
            _webSocketService = webSocketService;
        }

        [HttpGet("getAllFiles")]
        public async Task<ActionResult<Responses>> GetAllFilesInRoom([FromQuery] LongKeyCondition longKeyCondition)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var listFiles = await _fileService.GetListFilesOfClassRoomAsync(longKeyCondition.SearchKey);
                    return this.Success(listFiles, MsgNo.SUCCESS_GET_LIST_FILES);
                }

                return BadRequest(this.Error(MsgNo.ERROR_TOKEN_INVALID));
            }
            catch (Exception e)
            {
                return BadRequest(this.Error(e.Message));
            }
        }

        [HttpPost("addFileInClass")]
        public async Task<ActionResult<Responses>> AddFileInClass(AddFileDto addFileDto)
        {
            try
            {
                if (HttpContext.Items.TryGetValue("UserId", out var userIdObj) && userIdObj is string userId)
                {
                    var fileId = await _fileService.AddFileAsync(userId, addFileDto);
                    var websocketDataMsg = new SendWebSocketData
                    {
                        DataType = WebSocketDataType.IsNotification.ToString(),
                        Notification = new SendNotificationDto
                        {
                            NotificationType = NotificationType.IsPostFile.ToString(),
                            FileId = fileId,
                        }
                    };

                    await _webSocketService.SendNotification(websocketDataMsg, userId);
                    return this.Success(fileId, MsgNo.SUCCESS_UP_FILE);
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