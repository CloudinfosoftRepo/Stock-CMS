using AutoMapper;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.Repository
{
	public class GenratedFormRepository : EfRepository<TblGenratedForm, GenratedFormDto>, IGenratedFormRepository
	{
        private readonly StockCmsContext _dbContext;
        private readonly IMapper _mapper;
        public GenratedFormRepository(StockCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<GenratedFormDto> GetGenratedFormById(long Id)
        {
            return await GetOne(x => x.Id == Id);
        }  
        public async Task<IEnumerable<GenratedFormDto>> GetGenratedForm()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<GenratedFormDto>> GenrateForm(IEnumerable<GenratedFormDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<GenratedFormDto>> UpdateGenratedForm(IEnumerable<GenratedFormDto> data)
        {

            return await UpdateEntities(data);
        }


    }
}
