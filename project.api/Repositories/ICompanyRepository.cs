using project.models.Companies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<GetCompanyModel>> GetCompanies();
        Task<GetCompanyModel> GetCompany(string id);
        Task<GetCompanyModel> PostCompany(PostCompanyModel postCompanyModel);
        Task PutCompany(string id, PutCompanyModel putCompanyModel);
        Task DeleteCompany(string id);
    }
}
