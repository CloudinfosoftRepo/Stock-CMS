using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IHolderDocRepository
    {
        Task<IEnumerable<HolderDocsDto>> GetHolderDocById(long Id);
        Task<IEnumerable<HolderDocsDto>> GetHolderDocByHolderId(long holderId);
        Task<IEnumerable<HolderDocsDto>> GetHolderDocByLegalHeirId(long legalheirid);
        Task<IEnumerable<HolderDocsDto>> GetHolderDocByInfo(HolderDocsDto data);
        Task<IEnumerable<HolderDocsDto>> GetHolderDocByName(string Name);
        Task<IEnumerable<HolderDocsDto>> GetHolderDoc();
        Task<IEnumerable<HolderDocsDto>> AddHolderDoc(IEnumerable<HolderDocsDto> data);
        Task<IEnumerable<HolderDocsDto>> UpdateHolderDoc(IEnumerable<HolderDocsDto> data);

    }
}
