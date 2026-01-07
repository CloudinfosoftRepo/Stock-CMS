using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class BankRepository : EfRepository<TblBank, BankDto>, IBankRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;
        public BankRepository(DmCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BankDto>> AddBank(IEnumerable<BankDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<BankDto>> UpdateBank(IEnumerable<BankDto> data)
        {

            return await UpdateEntities(data);
        }
        public async Task<IEnumerable<BankDto>> UpdateBankbyColumn(IEnumerable<BankDto> data, string[] columns)
        {
            return await UpdateEntitiesArray(data, columns);
        }
        public async Task<IEnumerable<BankDto>> GetBankByClientId(long Id)
        {
            return await GetMany(x => x.ClientId == Id && x.IsActive == true);
        }

        public async Task<IEnumerable<BankDto>> GetBankByLegalHeirId(long Id)
        {
            return await GetMany(x => x.LegalHeirId == Id && x.IsActive == true);
        }
        public async Task<BankDto> GetBankById(long Id)
        {
            return await GetOne(x => x.Id == Id && x.IsActive == true);
        }
    }
}
