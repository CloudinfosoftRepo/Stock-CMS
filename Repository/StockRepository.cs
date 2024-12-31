using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Data;
using Stock_CMS.Entity;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class StockRepository : EfRepository<TblStock, StockDto>, IStockRepository
	{
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public StockRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StockDto>> GetStockById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }
       
        public async Task<IEnumerable<StockDto>> GetStock()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<StockDto>> AddStock(IEnumerable<StockDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<StockDto>> UpdateStock(IEnumerable<StockDto> data)
        {

            return await UpdateEntities(data);
        }


    }
}
