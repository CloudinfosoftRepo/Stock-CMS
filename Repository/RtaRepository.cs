using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class RtaRepository : EfRepository<TblRta, RtaDto>, IRtaRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public RtaRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RtaDto>> GetRtaById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }  
        public async Task<IEnumerable<RtaDto>> GetRtaByName(string Name)
        {
            return await GetMany(x => x.RtaName.ToLower() == Name.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<RtaDto>> GetRta()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<RtaDto>> AddRta(IEnumerable<RtaDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<RtaDto>> UpdateRta(IEnumerable<RtaDto> data)
        {
            return await UpdateEntities(data);
        }


    }
}
