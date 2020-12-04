using Microsoft.Extensions.DependencyInjection;

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
