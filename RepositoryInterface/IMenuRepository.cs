using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IMenuRepository
    {
        Task<IEnumerable<MenuDto>> GetMenu();
Task<IEnumerable<MenuDto>> GetMenuWithParents();

    }
}
