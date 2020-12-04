using Microsoft.AspNetCore.Builder;

namespace project.web.Middlewares
{
    public static class StateManagementMiddlewareExtensions
    {
        public static IApplicationBuilder UseStateManagement(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<StateManagementMiddleware>();
        }
    }
}
