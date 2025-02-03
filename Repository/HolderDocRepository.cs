using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class HolderDocRepository : EfRepository<TblHolderDoc, HolderDocsDto>, IHolderDocRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public HolderDocRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<HolderDocsDto>> GetHolderDocById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }
        public async Task<IEnumerable<HolderDocsDto>> GetHolderDocByHolderId(long holderId)
        {
            return await GetMany(x => x.HolderId == holderId && x.IsActive == true);
        }
        public async Task<IEnumerable<HolderDocsDto>> GetHolderDocByInfo(HolderDocsDto data)
        {
            return await GetMany(x => x.Id == data.Id && x.DocumentName == data.DocumentName && x.IsActive == true);
        }

        public async Task<IEnumerable<HolderDocsDto>> GetHolderDocByName(string DocumentName)
        {
            return await GetMany(x => x.DocumentName.ToLower() == DocumentName.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<HolderDocsDto>> GetHolderDoc()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<HolderDocsDto>> AddHolderDoc(IEnumerable<HolderDocsDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<HolderDocsDto>> UpdateHolderDoc(IEnumerable<HolderDocsDto> data)
        {

            return await UpdateEntities(data);
        }


    }
}
