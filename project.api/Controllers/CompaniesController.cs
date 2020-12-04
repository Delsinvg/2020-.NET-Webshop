using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Companies;
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
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;

        public CompaniesController(ICompanyRepository companyRepository)
        {
            _companyRepository = companyRepository;
        }

        /// <summary>
        /// Get a list of all companies.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/companies
        ///
        /// </remarks>
        /// <returns>List of GetCompanyModel</returns>
        /// <response code="200">Returns the list of companies</response>
        /// <response code="404">No companies were found</response>  

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<List<GetCompanyModel>>> GetCompanies()
        {
            try
            {
                List<GetCompanyModel> companies = await _companyRepository.GetCompanies();

                return companies;
            }
            catch (ProjectException e)
            {
                return NotFound(e.ProjectError);
            }
        }

        /// <summary>
        /// Get details of an company.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /api/companies/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <returns>An GetCompanyModel</returns>
        /// <response code="200">Returns the company</response>
        /// <response code="404">The company could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult<GetCompanyModel>> GetCompany(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid companyId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "GetCompany", "400");
                }

                GetCompanyModel company = await _companyRepository.GetCompany(companyId);

                return company;
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
        /// Creates an company.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /api/companies
        ///     {
        ///        "Name": "Name of the company"
        ///        "email": "Email of the company"
        ///        "accountnumber": "accountnumber of the company"
        ///        "addressId": "Id of the address"
        ///     }
        ///
        /// </remarks>
        /// <param name="postCompanyModel"></param>
        /// <returns>A newly created company</returns>
        /// <response code="201">Returns the newly created company</response>
        /// <response code="400">If something went wrong while saving into the database</response>  

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        //[Authorize(Roles = "Moderator")]
        public async Task<ActionResult<GetCompanyModel>> PostCompany(PostCompanyModel postCompanyModel)
        {
            try
            {
                GetCompanyModel company = await _companyRepository.PostCompany(postCompanyModel);

                return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
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
        /// Updates an company.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     PUT /api/companies/{id}
        ///     {
        ///        "Name": "Name of the company",
        ///        "email": "Email of the company"
        ///        "accountnumber": "accountnumber of the company"
        ///        "addressId": "Id of the address"
        ///     }
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <param name="putCompanyModel"></param>   
        /// <response code="204">Returns no content</response>
        /// <response code="404">The company could not be found</response> 
        /// <response code="400">The id is not a valid Guid or something went wrong while saving into the database</response>

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Moderator")]
        public async Task<IActionResult> PutCompany(string id, PutCompanyModel putCompanyModel)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid companyId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "PutCompany", "400");
                }

                await _companyRepository.PutCompany(companyId, putCompanyModel);

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
        /// Deletes an company.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     DELETE /api/companies/{id}
        ///
        /// </remarks>
        /// <param name="id"></param>      
        /// <response code="204">Returns no content</response>
        /// <response code="404">The company could not be found</response> 
        /// <response code="400">The id is not a valid Guid</response> 

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        //[Authorize(Roles = "Moderator")]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid companyId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteCompany", "400");
                }

                await _companyRepository.DeleteCompany(companyId);

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
