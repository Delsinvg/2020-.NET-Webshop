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

        [HttpPatch("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PatchUser(string id, PatchUserModel patchUserModel)
        {
            await _userRepository.PatchUser(id, patchUserModel);

            return NoContent();
        }

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
