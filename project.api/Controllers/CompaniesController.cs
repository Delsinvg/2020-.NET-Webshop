using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using project.api.Exceptions;
using project.api.Repositories;
using project.models.Companies;
using project.models.Products;

namespace project.api.Controllers
{
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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<GetCompanyModel>> PostBoek(PostCompanyModel postBoekModel)
        {
            try
            {
                GetCompanyModel company = await _companyRepository.PostCompany(postBoekModel);

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

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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

        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteCompany(string id)
        {
            try
            {
                if (!Guid.TryParse(id, out Guid boekId))
                {
                    throw new GuidException("Invalid Guid format.", this.GetType().Name, "DeleteCompany", "400");
                }

                await _companyRepository.DeleteCompany(boekId);

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
