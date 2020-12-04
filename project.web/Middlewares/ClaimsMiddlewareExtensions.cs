using Microsoft.AspNetCore.Builder;

namespace project.web.Middlewares
{
    public static class ClaimsMiddlewareExtensions
    {
        public static IApplicationBuilder UseClaims(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ClaimsMiddleware>();
        }
    }
}
