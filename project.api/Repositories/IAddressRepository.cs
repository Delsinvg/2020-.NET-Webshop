using project.models.Addresses;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface IAddressRepository
    {
        Task<List<GetAddressModel>> GetAddresses();
        Task<GetAddressModel> GetAddress(Guid id);
        Task<GetAddressModel> PostAddress(PostAddressModel postAddressModel);
        Task PutAddress(Guid id, PutAddressModel putAddressModel);
        Task DeleteAddress(Guid id);
    }
}
