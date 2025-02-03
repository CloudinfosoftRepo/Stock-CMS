using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface ICompanyService
    {
        Task<int> AddCompany(CompanyDto data);
        Task<Int32> UpdateCompany(CompanyDto data);
        Task<IEnumerable<CompanyDto>> GetCompany();

    }
}
