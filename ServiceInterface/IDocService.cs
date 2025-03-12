using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
	public interface IDocService
	{
		Task<long> AddDoc(DocDto data);
		Task<Int32> UpdateDoc(DocDto data);
		Task<IEnumerable<DocDto>> GetDocByClientId(long id);

		Task<DocDto> GetDocById(long id);

		Task<Int32> UpdateWitnessJson(long id, string jsonString, int updatedBy);


    }
}
