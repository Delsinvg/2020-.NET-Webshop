using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class CompanyRepository : ICompanyRepository
    {
        private readonly ProjectContext _context;

        public CompanyRepository(ProjectContext context)
        {
            _context = context;
        }

        public async Task DeleteCompany(Guid id)
        {
            try
            {
                Company company = await _context.Companies.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == id);

                if (company == null)
                {
                    throw new EntityException("Company not found.", this.GetType().Name, "DeleteCompany", "404");
                }

                _context.Companies.Remove(company);

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PutCompany", "400");
            }
        }
        

    public async Task<List<GetCompanyModel>> GetCompanies()
    {

        List<GetCompanyModel> companies = await _context.Companies
            .Select(x => new GetCompanyModel
            {
                Id = x.Id,
                Name = x.Name,
                Email = x.Email,
                AccountNumber = x.AccountNumber,
                Address = $"{x.Address.Country} ({x.Address.CountryCode}) - {x.Address.City} ({x.Address.PostalCode} {x.Address.Street})"
            })
            .AsNoTracking()
            .ToListAsync();


        if (companies.Count == 0)
        {
            throw new CollectionException("No companies found", this.GetType().Name, "GetCompanies", "404");
        }

        return companies;
    }
    

        public async Task<GetCompanyModel> GetCompany(Guid id)
        {
            
                GetCompanyModel company = await _context.Companies
                    .Select(x => new GetCompanyModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        AccountNumber = x.AccountNumber,
                        Address = $"{x.Address.Country} ({x.Address.CountryCode}) - {x.Address.City} ({x.Address.PostalCode}) {x.Address.Street}"

                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == id);

            if (company == null)
            {
                throw new EntityException("Company not found.", this.GetType().Name, "GetAuteur", "404");
            }
                
                return company;
            
        }

        public async Task<GetCompanyModel> PostCompany(PostCompanyModel postCompanyModel)
        {
            EntityEntry<Company> result = await _context.Companies.AddAsync(new Company
            {
                Name = postCompanyModel.Name,
                Email = postCompanyModel.Email,
                AccountNumber = postCompanyModel.AccountNumber,
                AddressId = postCompanyModel.AddressId
            });

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PostCompany", "400");
            }


            return await GetCompany(result.Entity.Id);
            
        }

        public async Task PutCompany(Guid id, PutCompanyModel putCompanyModel)
        {
            try
            {
                Company company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == id);

                if (company == null)
                {
                    throw new KeyNotFoundException();
                }

                company.Name = putCompanyModel.Name;
                company.Email = putCompanyModel.Email;
                company.AccountNumber = putCompanyModel.AccountNumber;
                company.AddressId = putCompanyModel.AddressId;

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PutAddress", "400");
            }
        }
    }
}
