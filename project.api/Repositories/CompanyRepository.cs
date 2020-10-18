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

        public async Task DeleteCompany(string id)
        {
            try
            {
                Company company = await _context.Companies.Include(x => x.Address).FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (company == null)
                {
                    throw new KeyNotFoundException();
                }

                _context.Companies.Remove(company);

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteCompany");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "DeleteCompany");
                }
            }
        }

        public async Task<List<GetCompanyModel>> GetCompanies()
        {
            try
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

                return companies;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetCompanies");
            }
        }

        public async Task<GetCompanyModel> GetCompany(string id)
        {
            try
            {
                GetCompanyModel company = await _context.Companies
                    .Select(x => new GetCompanyModel
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Email = x.Email,
                        AccountNumber = x.AccountNumber,
                        Address = $"{x.Address.Country} ({x.Address.CountryCode}) - {x.Address.City} ({x.Address.PostalCode} {x.Address.Street})"

                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return company;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetCompany");
            }
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

            await _context.SaveChangesAsync();

            return new GetCompanyModel
            {
                Id = result.Entity.Id,
                Name = result.Entity.Name,
                Email = result.Entity.Email,
                AccountNumber = result.Entity.AccountNumber,
                Address = $"{result.Entity.Address.Country} ({result.Entity.Address.CountryCode}) - {result.Entity.Address.City} ({result.Entity.Address.PostalCode} {result.Entity.Address.Street})"
            };
        }

        public async Task PutCompany(string id, PutCompanyModel putCompanyModel)
        {
            try
            {
                Company company = await _context.Companies.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (company == null)
                {
                    throw new KeyNotFoundException();
                }

                company.Name = putCompanyModel.Name;
                company.Email = putCompanyModel.Email;
                company.AccountNumber = putCompanyModel.AccountNumber;
                company.AddressId = putCompanyModel.AddressId;

                await _context.SaveChangesAsync();
            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }

                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutCompany");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "PutCompany");
                }
            }
        }
    }
}
