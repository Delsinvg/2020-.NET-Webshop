using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace project.web.Handlers
{
    public class ValidateHeaderHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly List<Uri> AnonymousEndpoints = new List<Uri>
        {
            new Uri("https://localhost:44388/api/Users"),
            new Uri("https://localhost:44388/api/Users/authenticate"),
            new Uri("https://localhost:44388/api/Users/SendResetToken"),
            new Uri("https://localhost:44388/api/Users/validatePasswordReset"),
            new Uri("https://localhost:44388/api/Users/refresh-token")
        };

        public ValidateHeaderHandler(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Add JWT token to authorization header for non-anonymous API endpoints
            if (!(request.Method.Equals(HttpMethod.Post) && AnonymousEndpoints.Contains(request.RequestUri)))
            {
                // No JWT token in session so authentication with credentials is required
                if (string.IsNullOrEmpty(_httpContextAccessor.HttpContext.Session.GetString("_JwtToken")))
                {
                    return new HttpResponseMessage(HttpStatusCode.Unauthorized);
                }

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _httpContextAccessor.HttpContext.Session.GetString("_JwtToken"));
            }

            // Add refresh token as a cookie to the request when a new set of tokens is being requested
            if (request.RequestUri.Equals(new Uri("https://localhost:44388/api/Users/refresh-token")))
            {
                request.Headers.Add("Cookie", "Project.RefreshToken=" + _httpContextAccessor.HttpContext.Session.GetString("_RefreshToken"));
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
