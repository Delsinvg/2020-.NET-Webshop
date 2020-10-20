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

        
        public async Task DeleteAddress(Guid id)
        {
            try
            {
                Address address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

                _context.Addresses.Remove(address);

                await _context.SaveChangesAsync();
            }
            catch (ProjectException e)
            {
                if (e.GetType().Name.Equals("KeyNotFoundException"))
                {
                    throw new KeyNotFoundException();
                }
                if (e.InnerException.GetType().Name.Equals("FormatException"))
                {
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "DeleteAddress", "400");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "DeleteAddress", "400");
                }
            }
        }

        public async Task<GetAddressModel> GetAddress(Guid id)
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
                    .FirstOrDefaultAsync(x => x.Id == id);

                return address;
            } catch (Exception e)
            {
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetAddresses", "400");
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
                throw new GuidException(e.InnerException.Message, this.GetType().Name, "GetAddresses", "400");
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch(Exception e)
            {
                throw new DatabaseException(e.InnerException.Message, this.GetType().Name, "PostAddress", "400");
            }
            

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

        public async Task PutAddress(Guid id, PutAddressModel putAddressModel)
        {
            try
            {
                Address address = await _context.Addresses.FirstOrDefaultAsync(x => x.Id == id);

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
                    throw new GuidException(e.InnerException.Message, this.GetType().Name, "PutAddress", "400");
                }

                if (e.GetType().ToString().Contains("DbUpdate"))
                {
                    throw new DatabaseException( e.InnerException.Message, this.GetType().Name, "PutAddress", "400");
                }
            }
        }
    }

}
