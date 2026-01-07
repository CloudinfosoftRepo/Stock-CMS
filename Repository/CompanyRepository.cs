using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class CompanyRepository : EfRepository<TblCompany, CompanyDto>, ICompanyRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;
        public CompanyRepository(DmCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CompanyDto>> GetCompanyById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }  
        public async Task<IEnumerable<CompanyDto>> GetCompanyByName(string Name)
        {
            return await GetMany(x => x.CompanyName.ToLower() == Name.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<CompanyDto>> GetCompany()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<CompanyDto>> AddCompany(IEnumerable<CompanyDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<CompanyDto>> UpdateCompany(IEnumerable<CompanyDto> data)
        {
            return await UpdateEntities(data);
        }

        public async Task<IEnumerable<CompanyDto>> GetCompanyByIds(long?[] ids)
        {
            return await GetMany(x => ids.Contains(x.Id));
        }
    }
}
