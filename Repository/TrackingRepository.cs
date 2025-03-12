using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class TrackingRepository : EfRepository<TblTracking, TrackingDto>, ITrackingRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public TrackingRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrackingDto>> GetTrackingById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }  
     
        public async Task<IEnumerable<TrackingDto>> GetTracking()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<TrackingDto>> GetTrackingbyStockId(long Id)
        {
            return await GetMany(x => x.IsActive == true && x.StockId == Id);
        }
        public async Task<IEnumerable<TrackingDto>> AddTracking(IEnumerable<TrackingDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<TrackingDto>> UpdateTracking(IEnumerable<TrackingDto> data)
        {
            return await UpdateEntities(data);
        }

        public async Task<IEnumerable<TrackingDto>> UpdateFormbyColumn(IEnumerable<TrackingDto> data, string[] columns)
        {
            return await UpdateEntitiesArray(data, columns);
        }

        public async Task<IEnumerable<TrackingDto>> GetTrackingbyStockIds(long[] Ids)
        {
            return await GetMany(x => x.IsActive == true && Ids.Contains(x.StockId));
        }
    }
}
