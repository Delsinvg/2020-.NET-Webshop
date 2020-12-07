using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Roles;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleRepository _roleRepository;

        public RolesController(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }

        /// <summary>
        /// Get a list of all roles.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/roles
        ///
        /// </remarks>
        /// <returns>List of GetRoleModel</returns>
        /// <response code="200">Returns the list of roles</response>
        /// <response code="404">No roles were found</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<List<GetRoleModel>>> GetRoles()
        {
            List<GetRoleModel> roles = await _roleRepository.GetRoles();

            if (roles.Count.Equals(0))
            {
                return NotFound();
            }

            return roles;
        }

        /// <summary>
        /// Get details of an role.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/roles/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>   
        /// <returns>An GetRoleModel</returns>
        /// <response code="200">Returns the role</response>
        /// <response code="404">The role could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<GetRoleModel>> GetRole(string id)
        {
            try
            {
                GetRoleModel role = await _roleRepository.GetRole(id);

                if (role == null)
                {
                    return NotFound();
                }

                return role;
            }
            catch (GuidException e)
            {
                return BadRequest(e.ProjectError);
            }
        }

        /// <summary>
        /// Creates an role.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/roles
        ///     {
        ///        "name": "Moderator"
        ///        "description": "Moderator"
        ///     }
        ///
        /// </remarks>
        /// <param name="postRoleModel"></param>
        /// <returns>A newly created role</returns>
        /// <response code="201">Returns the newly created role</response>
        /// <response code="400">If something went wrong while saving into the database</response> 
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<GetRoleModel>> PostRole(PostRoleModel postRoleModel)
        {
            try
            {
                GetRoleModel role = await _roleRepository.PostRole(postRoleModel);

                return CreatedAtAction(nameof(GetRole), new { id = role.Id }, role);
            }
            catch (Exception e)
            {
                return BadRequest(new { origin = e.Message, message = e.InnerException.Message });
            }
        }

        /// <summary>
        /// Updates an role.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/roles/{id}
        ///     {
        ///        "name": "Moderator"
        ///        "description": "Moderator"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>   
        /// <param name="putRoleModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The role could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> PutRole(string id, PutRoleModel putRoleModel)
        {
            try
            {
                await _roleRepository.PutRole(id, putRoleModel);

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
        /// Deletes an role.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/roles/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The role could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            try
            {
                await _roleRepository.DeleteRole(id);

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
