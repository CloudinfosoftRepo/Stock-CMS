using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class LegalHeirRepository : EfRepository<TblLegalHeir, LegalHeirDto>, ILegalHeirRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public LegalHeirRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByInfo(LegalHeirDto data)
        {
            return await GetMany(x => x.IsActive == true && x.Aadhar == data.Aadhar);
        }
        public async Task<IEnumerable<LegalHeirDto>> AddLegalHeir(IEnumerable<LegalHeirDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<LegalHeirDto>> UpdateLegalHeir(IEnumerable<LegalHeirDto> data)
        {
            return await UpdateEntities(data);
        }
        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirById(long Id)
        {
            return await GetMany(x => x.Id == Id && x.IsActive == true);
        }

        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByAadhar(string aadhar)
        {
            return await GetMany(x => x.Aadhar == aadhar && x.IsActive == true);
        }

        public async Task<IEnumerable<LegalHeirDto>> GetLegalHeirByClientId(long Id)
        {
            return await GetMany(x => x.DocId == Id && x.IsActive ==true);
        }

        public async Task<IEnumerable<LegalHeirDto>> UpdateLegalHeirbyColumn(IEnumerable<LegalHeirDto> data, string[] columns)
        {
            return await UpdateEntitiesArray(data, columns);
        }
    }
}
