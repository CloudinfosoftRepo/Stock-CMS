using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class FormRepository : EfRepository<TblForm, FormDto>, IFormRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;
        public FormRepository(DmCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<FormDto>> GetForms()
        {
            return await GetAllEntities();
        }
    }
}
