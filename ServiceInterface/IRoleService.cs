using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IRoleService
    {
        Task<string> AddRole(RoleDto role, int flag);
        Task<IEnumerable<RoleDto>> GetRole();


    }
}
