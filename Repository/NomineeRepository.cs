using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class NomineeRepository : EfRepository<TblNominee, NomineeDto>, INomineeRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;
        public NomineeRepository(DmCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<NomineeDto>> GetNomineeById(long Id)
        {
            return await GetMany(x => x.Id == Id);
        }

        public async Task<NomineeDto> GetOneNomineeById(long Id)
        {
            return await GetOne(x => x.Id == Id);
        }
        public async Task<IEnumerable<NomineeDto>> GetNomineeByInfo(NomineeDto data)
        {
            return await GetMany(x => x.Id == data.Id && x.Name == data.Name && x.IsActive == true);
        }
        public async Task<IEnumerable<NomineeDto>> GetNomineeByClientId(long Id)
        {
            return await GetMany(x => x.CustomerId == Id && x.IsActive == true);
        }

        public async Task<IEnumerable<NomineeDto>> GetNomineeByName(string Name)
        {
            return await GetMany(x => x.Name.ToLower() == Name.ToLower() && x.IsActive == true);
        }
        public async Task<IEnumerable<NomineeDto>> GetNominee()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<NomineeDto>> AddNominee(IEnumerable<NomineeDto> data)
        {
            return await AddEntitiesNotMap(data);
        }
        public async Task<IEnumerable<NomineeDto>> UpdateNominee(IEnumerable<NomineeDto> data)
        {
            return await UpdateEntities(data);
        }

        public async Task<IEnumerable<NomineeDto>> UpdateNomineeByColumnn(IEnumerable<NomineeDto> data, string[] columns)
        {
            return await UpdateEntitiesArray(data, columns);
        }

        public async Task<IEnumerable<NomineeDto>> GetNomineeByIds(long?[] ids)
        {
            return await GetMany(x => ids.Contains(x.Id));
        }
    }
}
