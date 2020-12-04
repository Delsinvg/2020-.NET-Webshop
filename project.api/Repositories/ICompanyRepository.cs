using project.models.Companies;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace project.api.Repositories
{
    public interface ICompanyRepository
    {
        Task<List<GetCompanyModel>> GetCompanies();
        Task<GetCompanyModel> GetCompany(Guid id);
        Task<GetCompanyModel> PostCompany(PostCompanyModel postCompanyModel);
        Task PutCompany(Guid id, PutCompanyModel putCompanyModel);
        Task DeleteCompany(Guid id);
    }
}
