using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IHolderDocService
    {
        Task<long> AddHolderDoc(HolderDocsDto data);
        Task<Int32> UpdateHolderDoc(HolderDocsDto data);
        Task<IEnumerable<HolderDocsDto>> GetHolderDocByHolderId(long holderId);

        Task<IEnumerable<HolderDocsDto>> GetHolderDocByLegalHeirId(long holderId);

    }
}
