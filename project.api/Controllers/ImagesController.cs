using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Images;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    //[Authorize(AuthenticationSchemes = "Bearer")]
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

        /// <summary>
        /// Get a list of all images.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/images
        ///
        /// </remarks>
        /// <returns>List of GetImageModel</returns>
        /// <response code="200">Returns the list of images</response>
        /// <response code="404">No auteurs were found</response> 
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[AllowAnonymous]
        public async Task<ActionResult<List<GetImageModel>>> GetImages()
        {
            try
            {
                List<GetImageModel> images = await _imageRepository.GetImages();

                return images;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        /// <summary>
        /// Get details of an image.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/images/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <returns>An GetImageModel</returns>
        /// <response code="200">Returns the image</response>
        /// <response code="404">The image could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[AllowAnonymous]
        public async Task<ActionResult<GetImageModel>> GetImage(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid imageId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetImage", "400");
                }

                GetImageModel image = await _imageRepository.GetImage(imageId);

                return image;
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

        /// <summary>
        /// Creates an image.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/images
        ///     {
        ///        "path": "Path of the image"
        ///        "productId": "Id of the product"
        ///     }
        ///
        /// </remarks>
        /// <param name="postImageModel"></param>
        /// <returns>A newly created image</returns>
        /// <response code="201">Returns the newly created image</response>
        /// <response code="400">If something went wrong while saving into the database</response> 

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Moderator")]
        public async Task<ActionResult<GetImageModel>> PostImage(PostImageModel postImageModel)
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

        /// <summary>
        /// Updates an image.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/images/{id}
        ///     {
        ///        "path": "Path of the image"
        ///        "productId": "Id of the product"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <param name="putImageModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The image could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response> 

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Moderator")]
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

        /// <summary>
        /// Deletes an image.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/images/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <response code="204">Returns no content</response>
        /// <response code="404">The image could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response>

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Moderator")]
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
