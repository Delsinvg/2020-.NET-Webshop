using Microsoft.AspNetCore.Http;
using project.web.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Middlewares
{
    public class StateManagementMiddleware
    {
        private readonly RequestDelegate _next;

        public StateManagementMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IStateManagementService stateManagementService)
        {
            if (context.Request.Cookies.ContainsKey("Project.RememberMe"))
            {
                stateManagementService.SetSession(context.Request.Cookies["Project.RememberMe"]);
            }

            // Call the next delegate/middleware in the pipeline
            await _next(context);
        }
    }
}
