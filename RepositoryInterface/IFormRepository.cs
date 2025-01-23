using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IFormRepository
    {
        Task<IEnumerable<FormDto>> GetForms();
    }
}
