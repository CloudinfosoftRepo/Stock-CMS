using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class RelationMappingRepository : EfRepository<TblRelationMapping, RelationMappingDto>, IRelationMappingRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RelationMappingRepository> _logger;
        public RelationMappingRepository(DmCmsContext dbContext, IMapper mapper, ILogger<RelationMappingRepository> logger) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<RelationMappingDto>> AddRelationMappings(IEnumerable<RelationMappingDto> data)
        {
            return await AddEntitiesNotMap(data);
        }

        public async Task<IEnumerable<RelationMappingDto>> UpdateRelationMappings(IEnumerable<RelationMappingDto> data)
        {
            return await UpdateEntities(data);
        }

        public async Task<IEnumerable<RelationMappingDto>> AddOrUpdateRelationMappings(IEnumerable<RelationMappingDto> data)
        {
            return await AddOrUpdateEntities(data);
        }

        public async Task<IEnumerable<RelationMappingDto>> UpdateeRelationMappingsByColumn(IEnumerable<RelationMappingDto> data, string[] columns)
        {
            return await UpdateEntitiesArray(data, columns);
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsById(long Id)
        {
            return await GetMany(x => x.Id == Id && x.IsActive == true);
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByHolderId(long Id)
        {
            return await GetMany(x => x.HolderId == Id && x.IsActive == true && (x.NomineeId == null || x.NomineeId == 0));
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByHolderIdAndNominee(long Id)
        {
            return await GetMany(x => x.HolderId == Id && x.IsActive == true && (x.LegalHeirId == 0 || x.LegalHeirId == null));
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByLegalHeirId(long Id)
        {
            return await GetMany(x => x.LegalHeirParentId == Id && x.IsActive == true && (x.NomineeId == null || x.NomineeId == 0));
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByLegalHeirIdAndNominee(long Id)
        {
            return await GetMany(x => x.LegalHeirParentId == Id && x.IsActive == true && (x.LegalHeirId == 0 || x.LegalHeirId == null));
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByHolderIds(long?[] Id)
        {
            return await GetMany(x => Id.Contains(x.HolderId) && x.IsActive == true);
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByLegalHeirIds(long?[] Id)
        {
            return await GetMany(x => Id.Contains(x.LegalHeirParentId) && x.IsActive == true);
        }

        public async Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByNomineeIds(long?[] Id)
        {
            return await GetMany(x => Id.Contains(x.NomineeId) && x.IsActive == true);
        }

        public async Task<IEnumerable<RelationMappingDto>> GetAllRelationMappings()
        {
            return await GetMany(x => x.IsActive == true);
        }
    }
}
