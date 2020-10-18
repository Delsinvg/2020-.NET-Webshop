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
        Task<GetAddressModel> GetAddress(string id);
        Task<GetAddressModel> PostAddress(PostAddressModel postAddressModel);
        Task PutAddress(string id, PutAddressModel putAddressModel);
        Task DeleteAddress(string id);
    }
}
