using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class DocRepository : EfRepository<TblDoc, DocDto>, IDocRepository
	{
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public DocRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<DocDto>> GetDocById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }  
        public async Task<IEnumerable<DocDto>> GetdocByClientId(long Id)
        {
            return await GetMany(x => x.CustomerId == Id);
        } 
        public async Task<IEnumerable<DocDto>> GetDocByInfo(DocDto data)
        {
            return await GetMany(x => x.Id == data.Id && x.Name == data.Name);
        }


        public async Task<IEnumerable<DocDto>> GetDocByName(string Name)
        {
            return await GetMany(x => x.Name.ToLower() == Name.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<DocDto>> GetDoc()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<DocDto>> AddDoc(IEnumerable<DocDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<DocDto>> UpdateDoc(IEnumerable<DocDto> data)
        {

            return await UpdateEntities(data);
        }


    }
}
