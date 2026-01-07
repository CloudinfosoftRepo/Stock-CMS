using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IRelationMappingRepository
    {
        Task<IEnumerable<RelationMappingDto>> AddRelationMappings(IEnumerable<RelationMappingDto> data);
        Task<IEnumerable<RelationMappingDto>> UpdateRelationMappings(IEnumerable<RelationMappingDto> data);

        Task<IEnumerable<RelationMappingDto>> AddOrUpdateRelationMappings(IEnumerable<RelationMappingDto> data);
        Task<IEnumerable<RelationMappingDto>> UpdateeRelationMappingsByColumn(IEnumerable<RelationMappingDto> data, string[] columns);
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsById(long Id);
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByHolderId(long Id);
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByLegalHeirId(long Id);
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByLegalHeirIdAndNominee(long Id);

        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByHolderIds(long?[] Id);
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByLegalHeirIds(long?[] Id);
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByNomineeIds(long?[] Id);
        Task<IEnumerable<RelationMappingDto>> GetAllRelationMappings();
        Task<IEnumerable<RelationMappingDto>> GetRelationMappingsByHolderIdAndNominee(long Id);

    }
}
