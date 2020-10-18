using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using project.api.Entities;
using project.api.Exceptions;
using project.models.Addresses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ProjectContext _context;

        public AddressRepository(ProjectContext context)
        {
            _context = context;
        }

        
        public async Task DeleteAddress(string id)
        {
            try
            {
                Address address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                _context.Addresses.Remove(address);

                await _context.SaveChangesAsync();
            }
            catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteAddress");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "DeleteAddress");
                }
            }
        }

        public async Task<GetAddressModel> GetAddress(string id)
        {
            try
            {
                GetAddressModel address = await _context.Addresses
                    .Select(x => new GetAddressModel
                    {
                        Id = x.Id,
                        CountryCode = x.CountryCode,
                        Country = x.City,
                        City = x.City,
                        PostalCode = x.PostalCode,
                        Street = x.Street
                    })
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                return address;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetAddress");
            }
        }

        public async Task<List<GetAddressModel>> GetAddresses()
        {
            try
            {
                List<GetAddressModel> addresses = await _context.Addresses
                    .Include(x => x.Company)
                    .Include(x => x.User)
                    .Select(x => new GetAddressModel
                    {
                        Id = x.Id,
                        CountryCode = x.CountryCode,
                        Country = x.City,
                        City = x.City,
                        PostalCode = x.PostalCode,
                        Street = x.Street,
                        Company = x.Company.Name,
                        User = $"{x.User.FirstName} {x.User.LastName}"
                    })
                    .AsNoTracking()
                    .ToListAsync();

                return addresses;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetAddresses");
            }
        }

        public async Task<GetAddressModel> PostAddress(PostAddressModel postAddressModel)
        {
            EntityEntry<Address> result = await _context.Addresses.AddAsync(new Address
            {
                CountryCode = postAddressModel.CountryCode,
                Country = postAddressModel.Country,
                City = postAddressModel.City,
                PostalCode = postAddressModel.PostalCode,
                Street = postAddressModel.Street,
            });

            await _context.SaveChangesAsync();

            return new GetAddressModel
            {
                Id = result.Entity.Id,
                CountryCode = result.Entity.CountryCode,
                Country = result.Entity.Country,
                City = result.Entity.City,
                PostalCode = result.Entity.PostalCode,
                Street = result.Entity.Street,
            };
        }

        public async Task PutAddress(string id, PutAddressModel putAddressModel)
        {
            try
            {
                Address address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == Guid.Parse(id));

                if (address == null)
                {
                    throw new KeyNotFoundException();
                }

                address.CountryCode = putAddressModel.CountryCode;
                address.Country = putAddressModel.Country;
                address.City = putAddressModel.City;
                address.PostalCode = putAddressModel.PostalCode;
                address.Street = putAddressModel.Street;

                await _context.SaveChangesAsync();

            } catch (Exception e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }

                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutAddress");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.GetType().Name, e.InnerException.Message, this.GetType().Name, "PutAddress");
                }
            }
        }
    }

}
