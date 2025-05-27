using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class MenuRepository : EfRepository<TblMenu, MenuDto>, IMenuRepository
    {
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;

        public MenuRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<IEnumerable<MenuDto>> GetMenu()
        {
            return await GetMany(x => x.ParentId != null);
        }
        public async Task<IEnumerable<MenuDto>> GetMenuWithParents()
        {
            return await GetMany(x => x.Id > 0);
        }
    }
}
