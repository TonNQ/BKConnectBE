using Hangfire.Dashboard;

namespace BKConnectBE.Filter
{
    public class DashboardAuthorizeFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            // chỉ vô được khi đã đăng nhập
            // var httpContext = context.GetHttpContext();
            // return httpContext.User.Identity?.IsAuthenticated ?? false;

            // mọi người đều vô được
            return true;
        }
    }
}