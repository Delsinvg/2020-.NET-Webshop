using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Services
{
    public static class ProjectServiceCollection
    {
        public static IServiceCollection AddProject(this IServiceCollection services)
        {
            services.AddScoped<IStateManagementService, StateManagementService>();
            services.AddScoped<ITokenValidationService, TokenValidationService>();

            return services;
        }
    }
}
