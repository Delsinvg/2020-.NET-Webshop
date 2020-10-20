using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using project.models.Addresses;

namespace project.api.Repositories
{
    public interface IAddressRepository
    {
        Task<List <GetAddressModel>> GetAddresses();
        Task<GetAddressModel> GetAddress(Guid id);
        Task<GetAddressModel> PostAddress(PostAddressModel postAddressModel);
        Task PutAddress(Guid id, PutAddressModel putAddressModel);
        Task DeleteAddress(Guid id);
    }
}
