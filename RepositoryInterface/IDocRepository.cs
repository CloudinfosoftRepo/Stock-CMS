using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
	public interface IDocRepository
	{
		Task<IEnumerable<DocDto>> GetDocById(long Id);

		Task<DocDto> GetOneDocById(long Id);

        Task<IEnumerable<DocDto>> GetdocByClientId(long Id);
		Task<IEnumerable<DocDto>> GetDocByInfo(DocDto data);
		Task<IEnumerable<DocDto>> GetDocByName(string Name);
		Task<IEnumerable<DocDto>> GetDoc();
		Task<IEnumerable<DocDto>> AddDoc(IEnumerable<DocDto> data);
		Task<IEnumerable<DocDto>> UpdateDoc(IEnumerable<DocDto> data);
		Task<IEnumerable<DocDto>> UpdateDocByColumnn(IEnumerable<DocDto> data, string[] columns);

        Task<IEnumerable<DocDto>> GetDocByIds(long?[] ids);

    }
}
