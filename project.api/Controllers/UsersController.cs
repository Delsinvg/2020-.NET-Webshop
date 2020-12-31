using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.RefreshTokens;
using project.models.Users;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Get a list of all users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/users
        ///
        /// </remarks>
        /// <returns>List of GetUserModel</returns>
        /// <response code="200">Returns the list of users</response>
        /// <response code="404">No users were found</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<List<GetUserModel>>> GetUsers()
        {
            List<GetUserModel> users = await _userRepository.GetUsers();

            if (users.Count.Equals(0))
            {
                return NotFound();
            }

            return users;
        }

        /// <summary>
        /// Get details of an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/users/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>     
        /// <returns>An GetUserModel</returns>
        /// <response code="200">Returns the user</response>
        /// <response code="404">The user could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<GetUserModel>> GetUser(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new GuidException("Ongeldig id", this.GetType().Name, "GetUser", "400");
                }

                GetUserModel user = await _userRepository.GetUser(userId.ToString());

                return user;
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else if (e.ProjectError.Status.Equals("403"))
                {
                    return new ObjectResult(e.ProjectError)
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Creates an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users
        ///     {
        ///        "firstname": "Jack"
        ///        "lastname": "Sparrow"
        ///        "email": "jack@sparrow.com"
        ///        "password": "Azerty123"
        ///        "addressId": "93afc959-0b65-49d0-97ee-9088ea36a4ca"
        ///        "roles": [
        ///            "Moderator"
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="postUserModel"></param>
        /// <returns>A newly created user</returns>
        /// <response code="201">Returns the newly created user</response>
        /// <response code="400">If something went wrong while saving into the database</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<GetUserModel>> PostUser(PostUserModel postUserModel)
        {
            try
            {
                GetUserModel user = await _userRepository.PostUser(postUserModel);

                return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Updates an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/users/{id}
        ///     {
        ///        "firstname": "Jack"
        ///        "lastname": "Sparrow"
        ///        "email": "jack@sparrow.com"
        ///        "password": "Azerty123"
        ///        "addressId": "93afc959-0b65-49d0-97ee-9088ea36a4ca"
        ///        "roles": [
        ///            "Moderator"
        ///        ]
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>     
        /// <param name="putUserModel"></param>    
        /// <response code="204">Returns no content</response>
        /// <response code="404">The user could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PutUser(string id, PutUserModel putUserModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new GuidException("Ongeldig id", this.GetType().Name, "PutUser", "400");
                }

                await _userRepository.PutUser(userId.ToString(), putUserModel);

                return NoContent();
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else if (e.ProjectError.Status.Equals("403"))
                {
                    return new ObjectResult(e.ProjectError)
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Updates an user password.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /api/users/{id}
        ///     {
        ///        "CurrentPassword": "Azerty123"
        ///        "NewPassword": "Azerty123*"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>     
        /// <param name="patchUserModel"></param>    
        /// <response code="204">Returns no content</response>
        /// <response code="404">The user could not be found</response> 
        /// <response code="400">The id is not a valid Guid or the current password does not match or the new password is not conform the password rules</response>
        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> PatchUser(string id, PatchUserModel patchUserModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new GuidException("Ongeldig id", this.GetType().Name, "PatchUser", "400");
                }

                await _userRepository.PatchUser(userId.ToString(), patchUserModel);

                return NoContent();
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else if (e.ProjectError.Status.Equals("403"))
                {
                    return new ObjectResult(e.ProjectError)
                    {
                        StatusCode = (int)HttpStatusCode.Forbidden
                    };
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Deletes an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/users/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>     
        /// <response code="204">Returns no content</response>
        /// <response code="404">The user could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new GuidException("Ongeldig id", this.GetType().Name, "DeleteUser", "400");
                }

                await _userRepository.DeleteUser(userId.ToString());

                return NoContent();
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        // JWT Action Methods
        // ==================

        /// <summary>
        /// Authenticates an user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/authenticate
        ///     {
        ///        "UserName": "blancquaert.yves@svsl.be",
        ///        "Password": "_Azerty123"
        ///     }
        ///
        /// </remarks>
        /// <param name="postAuthenticateRequestModel"></param>
        /// <returns>Details of authenticated user, an JWT token and a refresh token</returns>
        /// <response code="200">Returns the authenticated user with tokens</response>
        /// <response code="401">Incorrect credentials</response>   
        [AllowAnonymous]
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostAuthenticateResponseModel>> Authenticate(PostAuthenticateRequestModel postAuthenticateRequestModel)
        {
            try
            {
                PostAuthenticateResponseModel postAuthenticateResponseModel = await _userRepository.Authenticate(postAuthenticateRequestModel, IpAddress());

                SetTokenCookie(postAuthenticateResponseModel.RefreshToken);

                return postAuthenticateResponseModel;
            }
            catch (ProjectException e)
            {
                return Unauthorized(e.ProjectError);
            }
        }

        /// <summary>
        /// Renew tokens.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/refresh-token
        ///
        /// </remarks>
        /// <returns>Details of authenticated user, a new JWT token and a new refresh token</returns>
        /// <response code="200">Returns the authenticated user with new tokens</response>
        /// <response code="401">Invalid refresh token</response>   
        [AllowAnonymous]
        [HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<PostAuthenticateResponseModel>> RefreshToken()
        {
            try
            {
                string refreshToken = Request.Cookies["Project.RefreshToken"];

                PostAuthenticateResponseModel postAuthenticateResponseModel = await _userRepository.RefreshToken(refreshToken, IpAddress());

                SetTokenCookie(postAuthenticateResponseModel.RefreshToken);

                return postAuthenticateResponseModel;
            }
            catch (ProjectException e)
            {
                return Unauthorized(e.ProjectError);
            }
        }

        /// <summary>
        /// Revoke token.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/users/revoke-token
        ///     {
        ///        "Token": "Some token"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Disables a refresh token</response>
        /// <response code="400">No token present in body or cookie</response>   
        /// <response code="401">No user found with this token or refresh token is no longer active</response>   
        [HttpPost("revoke-token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> RevokeToken(PostRevokeTokenRequestModel postRevokeTokenRequestModel)
        {
            try
            {
                // Accept token from request body or cookie
                string token = postRevokeTokenRequestModel.Token ?? Request.Cookies["Project.RefreshToken"];

                if (string.IsNullOrEmpty(token))
                {
                    throw new RefreshTokenException("Refresh token is required.", this.GetType().Name, "RevokeToken", "400");
                }

                await _userRepository.RevokeToken(token, IpAddress());

                return Ok();
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("400"))
                {
                    return BadRequest(e.ProjectError);
                }
                else
                {
                    return Unauthorized(e.ProjectError);
                }
            }
        }

        /// <summary>
        /// Get a list of all refresh tokens of a user.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/users/{id}/refresh-tokens
        ///
        /// </remarks>
        /// <returns>List of GetRefreshTokenModel</returns>
        /// <response code="200">Returns the list of refresh tokens</response>
        /// <response code="404">No refresh tokens were found</response> 
        /// <response code="400">The id is not a valid Guid</response> 
        [HttpGet("{id}/refresh-tokens")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<List<GetRefreshTokenModel>>> GetUserRefreshTokens(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid userId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetUserRefreshTokens", "400");
                }

                List<GetRefreshTokenModel> refreshTokens = await _userRepository.GetUserRefreshTokens(userId);

                return refreshTokens;
            }
            catch (ProjectException e)
            {
                if (e.ProjectError.Status.Equals("404"))
                {
                    return NotFound(e.ProjectError);
                }
                else
                {
                    return BadRequest(e.ProjectError);
                }
            }
        }

        // JWT helper methods
        // ==================

        private void SetTokenCookie(string token)
        {
            CookieOptions cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("Project.RefreshToken", token, cookieOptions);
        }

        private string IpAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
            {
                return Request.Headers["X-Forwarded-For"];
            }
            else
            {
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            }
        }
    }
}
