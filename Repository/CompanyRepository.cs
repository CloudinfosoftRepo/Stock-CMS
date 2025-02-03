using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class CompanyRepository : EfRepository<TblCompany, CompanyDto>, ICompanyRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public CompanyRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
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


    }
}
