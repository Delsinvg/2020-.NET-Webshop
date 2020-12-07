using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Categories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [Consumes("application/json")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        /// Get a list of all categories.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/categories
        ///
        /// </remarks>
        /// <returns>List of GetCategoryModel</returns>
        /// <response code="200">Returns the list of categories</response>
        /// <response code="404">No categories were found</response>

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<ActionResult<List<GetCategoryModel>>> GetCategories()
        {
            try
            {
                List<GetCategoryModel> categories = await _categoryRepository.GetCategories();

                return categories;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        /// <summary>
        /// Get details of an category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/category/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <returns>An GetCategoryModel</returns>
        /// <response code="200">Returns the category</response>
        /// <response code="404">The category could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response>

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        public async Task<ActionResult<GetCategoryModel>> GetCategory(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid categoryId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetCategory", "400");
                }

                GetCategoryModel category = await _categoryRepository.GetCategory(categoryId);

                return category;
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
        /// Creates an category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/categories
        ///     {
        ///        "Name": "Name of the category"
        ///        "parentId": "Id of the parent category"
        ///     }
        ///
        /// </remarks>
        /// <param name="postCategoryModel"></param>
        /// <returns>A newly created category</returns>
        /// <response code="201">Returns the newly created category</response>
        /// <response code="400">If something went wrong while saving into the database</response>

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Moderator")]
        public async Task<ActionResult<GetCategoryModel>> PostCategory(PostCategoryModel postCategoryModel)
        {
            try
            {
                GetCategoryModel category = await _categoryRepository.PostCategory(postCategoryModel);

                return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
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
        /// Updates an category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/categories/{id}
        ///     {
        ///        "Name": "Name of the category"
        ///        "parentId": "Id of the parent category"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <param name="putCategoryModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The category could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> PutCategory(string id, PutCategoryModel putCategoryModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid categoryId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutCategory", "400");
                }

                await _categoryRepository.PutCategory(categoryId, putCategoryModel);

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
        /// Deletes an category.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/categories/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <response code="204">Returns no content</response>
        /// <response code="404">The category could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid categoryId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteCategory", "400");
                }

                await _categoryRepository.DeleteCategory(categoryId);

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
