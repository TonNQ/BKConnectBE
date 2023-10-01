namespace BKConnectBE.Common
{
    public static class Helper
    {
        public static string GetDomainName(IHttpContextAccessor httpContextAccessor)
        {
            HttpContext httpContext = httpContextAccessor.HttpContext;
            return $"{httpContext.Request.Scheme}://{httpContext.Request.Host.Value}";
        }
    }
}