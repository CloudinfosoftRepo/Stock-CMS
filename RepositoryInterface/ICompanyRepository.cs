using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
	public interface ICompanyRepository
	{
		Task<IEnumerable<CompanyDto>> GetCompanyById(long Id);
		Task<IEnumerable<CompanyDto>> GetCompanyByName(string Name);
		Task<IEnumerable<CompanyDto>> GetCompany();
		Task<IEnumerable<CompanyDto>> AddCompany(IEnumerable<CompanyDto> data);
		Task<IEnumerable<CompanyDto>> UpdateCompany(IEnumerable<CompanyDto> data);

	}
}
