using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
	public interface IDocService
	{
		Task<long> AddDoc(DocDto data);
		Task<Int32> UpdateDoc(DocDto data);

		Task<long> UpdateDocbyColumn(DocDto data);

        Task<IEnumerable<DocDto>> GetDocByClientId(long id);

		Task<DocDto> GetDocById(long id);

		Task<Int32> UpdateWitnessJson(long id, string jsonString, int updatedBy);

		Task<IEnumerable<RelationMappingDto>> GetRelationMapping(long id, string holdertype);

		Task<long> AddRelationMapping(IEnumerable<RelationMappingDto> data);

		Task<Int32> UpdateRelationMapping(string holdertype, IEnumerable<RelationMappingDto> data);
    }
}
