using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Model.Dtos.Authorizations;
using BKConnectBE.Services.Authorizations;
using Microsoft.AspNetCore.Mvc;

namespace BKConnectBE.Controllers.Authorizations
{
    [ApiController]
    [Route("author")]
    public class AuthorizationController : Controller
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            var responseInfo = new Responses();
            try
            {
                responseInfo = await _authorizationService.Register(registerDto);
            }
            catch
            {
                responseInfo.Code = ResponseCode.SERVER_ERROR;
                responseInfo.Message = MsgNo.ERROR_INTERNAL_SERVICE;
            }
            return Ok(responseInfo);
        }

        [HttpGet("active-account")]
        public async Task<IActionResult> ActiveAccount(string secretHash)
        {
            var responseInfo = new Responses();
            try
            {
                responseInfo = await _authorizationService.ActiveAccount(secretHash);
            }
            catch
            {
                responseInfo.Code = ResponseCode.SERVER_ERROR;
                responseInfo.Message = MsgNo.ERROR_INTERNAL_SERVICE;
            }
            return Ok(responseInfo);
        }
    }
}