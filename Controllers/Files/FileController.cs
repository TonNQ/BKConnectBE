using BKConnect.BKConnectBE.Common;
using BKConnect.Controllers;
using BKConnectBE.Common.Attributes;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Service.Files;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Files
{
    [CustomAuthorize]
    [ApiController]
    [Route("files")]
    public class FileController : ControllerBase
    {
        private readonly IFileService _fileService;
        public FileController(IFileService fileService)
        {
            _fileService = fileService;
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
    }
}