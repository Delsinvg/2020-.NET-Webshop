using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Entities;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Categories;

namespace project.api.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetCategoryModel>> GetCategory(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid categoryId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetCategory", "400");
                }

                GetCategoryModel boek = await _categoryRepository.GetCategory(categoryId);

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
        public async Task<ActionResult<GetCategoryModel>> PostBoek(PostCategoryModel postCategoryModel)
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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> PutCategory(string id, PutCategoryModel putCategoryModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid boekId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutBoek", "400");
                }

                await _categoryRepository.PutCategory(boekId, putCategoryModel);

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
        public async Task<IActionResult> DeleteBoek(string id)
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
