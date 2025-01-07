using Stock_CMS.Entity;

namespace Stock_CMS.ServiceInterface
{
	public interface IDocService
	{
		Task<long> AddDoc(DocDto data);
		Task<Int32> UpdateDoc(DocDto data);
		Task<IEnumerable<DocDto>> GetDocByClientId(long id);

	}
}
