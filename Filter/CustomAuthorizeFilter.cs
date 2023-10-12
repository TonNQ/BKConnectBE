using BKConnect.BKConnectBE.Common;
using BKConnect.Service.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace HK1_2023_2024.PBL4.BKConnectBE.Filter
{
    public class CustomAuthorizeFilter : IAuthorizationFilter
    {
        private readonly IJwtService _jwtService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CustomAuthorizeFilter(IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _jwtService = jwtService;
            _httpContextAccessor = httpContextAccessor;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var allowAnonymous = context.ActionDescriptor.EndpointMetadata.OfType<AllowAnonymousAttribute>().Any();
            if (allowAnonymous)
                return;

            if (_httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault() == null)
            {
                context.Result = new UnauthorizedObjectResult(new { message = MsgNo.ERROR_TOKEN_INVALID, data = "" });
                return;
            }
            try
            {
                string token = _httpContextAccessor.HttpContext.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last();

                string userId = _jwtService.ValidateToken(true, token);

                if (userId != null)
                {
                    context.HttpContext.Items["UserId"] = userId;
                }
            }
            catch (Exception e)
            {
                context.Result = new UnauthorizedObjectResult(new { message = e.Message, data = "" });
            }
        }
    }
}