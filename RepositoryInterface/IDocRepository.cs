using Stock_CMS.Entity;

namespace Stock_CMS.RepositoryInterface
{
	public interface IDocRepository
	{
		Task<IEnumerable<DocDto>> GetDocById(long Id);
		Task<IEnumerable<DocDto>> GetdocByClientId(long Id);
		Task<IEnumerable<DocDto>> GetDocByInfo(DocDto data);
		Task<IEnumerable<DocDto>> GetDocByName(string Name);
		Task<IEnumerable<DocDto>> GetDoc();
		Task<IEnumerable<DocDto>> AddDoc(IEnumerable<DocDto> data);
		Task<IEnumerable<DocDto>> UpdateDoc(IEnumerable<DocDto> data);

	}
}
