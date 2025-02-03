using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IRtaService
    {
        Task<int> AddRta(RtaDto data);
        Task<Int32> UpdateRta(RtaDto data);
        Task<IEnumerable<RtaDto>> GetRta();

    }
}
