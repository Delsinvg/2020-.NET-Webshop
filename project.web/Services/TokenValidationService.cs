using Microsoft.AspNetCore.Http;
using project.api.Exceptions;
using project.models.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.web.Services
{
    public class TokenValidationService : ITokenValidationService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ProjectApiService _projectApiService;
        private readonly IStateManagementService _stateManagementService;

        public TokenValidationService(
            IHttpContextAccessor httpContextAccessor,
            ProjectApiService projectApiService,
            IStateManagementService stateManagementService)
        {
            _httpContextAccessor = httpContextAccessor;
            _projectApiService = projectApiService;
            _stateManagementService = stateManagementService;
        }

        public async Task Validate(string sourceClass, string sourceMethod)
        {
            // Check if JWT token exists in session, if not redirect to login page
            if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("_JwtToken")))
            {
                throw new TokenException("You are not logged in", sourceClass, sourceMethod, "401");
            }

            // JWT exist in session, check if expired
            if (Convert.ToDateTime(_httpContextAccessor.HttpContext.Session.GetString("_JwtExpiresOn")) < DateTime.UtcNow)
            {
                // Unnecesary because check is done at back-end but this client-side check saves an API call
                // Check if refresh token is expired
                // If expired the user needs to authenticate with credentials
                if (Convert.ToDateTime(_httpContextAccessor.HttpContext.Session.GetString("_RtExpiresOn")) < DateTime.UtcNow)
                {
                    throw new TokenException("Your session has expired", sourceClass, sourceMethod, "401");
                }

                // If not expired a request to /api/Users/refresh-token is required to get a new set of tokens
                PostAuthenticateResponseModel postAuthenticateResponseModel = await _projectApiService.RefreshToken();

                // Update the session data with the new set of tokens
                string rememberMe = _httpContextAccessor.HttpContext.Request.Cookies.ContainsKey("Project.RememberMe") ? "true" : null;
                _stateManagementService.SetState(postAuthenticateResponseModel, rememberMe);
            }
        }
    }
}
