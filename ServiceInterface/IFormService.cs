using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IFormService
    {
        Task<IEnumerable<FormDto>> GetFormList();
    }
}
