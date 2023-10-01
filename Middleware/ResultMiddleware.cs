using BKConnect.BKConnectBE.Common;

namespace BKConnect.Middleware
{
    public class ResultMiddleware
    {
        private readonly RequestDelegate _next;

        public ResultMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                var response = new Responses()
                {
                    Data = exception.Message,
                    Message = MsgNo.ERROR_INTERNAL_SERVICE
                };

                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                await context.Response.WriteAsJsonAsync<Responses>(response);
            }
        }
    }
}
