using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Images;

namespace project.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<List<GetImageModel>>> GetImages()
        {
            try
            {
                List<GetImageModel> boeken = await _imageRepository.GetImages();

                return boeken;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetImageModel>> GetImage(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid imageId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetImage", "400");
                }

                GetImageModel boek = await _imageRepository.GetImage(imageId);

                return boek;
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetImageModel>> PostBoek(PostImageModel postImageModel)
        {
            try
            {
                GetImageModel image = await _imageRepository.PostImage(postImageModel);

                return CreatedAtAction(nameof(GetImage), new { id = image.Id }, image);
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutImage(string id, PutImageModel putImageModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid imageId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutImage", "400");
                }

                await _imageRepository.PutImage(imageId, putImageModel);

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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteImage(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid imageId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteImage", "400");
                }

                await _imageRepository.DeleteImage(imageId);

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
    }
}
