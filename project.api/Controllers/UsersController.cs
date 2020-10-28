using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Users;

namespace project.api.Controllers
{
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
        public async Task<ActionResult<GetUserModel>> GetUser(string id)
        {
            try
            {
                GetUserModel user = await _userRepository.GetUser(id);

                if (user == null)
                {
                    return NotFound();
                }

                return user;
            }
            catch (GuidException e)
            {
                return BadRequest(e.ProjectError);
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
        public async Task<IActionResult> PutUser(string id, PutUserModel putUserModel)
        {
            try
            {
                await _userRepository.PutUser(id, putUserModel);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
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
        public async Task<IActionResult> PatchUser(string id, PatchUserModel patchUserModel)
        {
            await _userRepository.PatchUser(id, patchUserModel);

            return NoContent();
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                await _userRepository.DeleteUser(id);

                return NoContent();
            }
            catch (ArgumentNullException)
            {
                return NotFound();
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
            }
        }
    }
}
