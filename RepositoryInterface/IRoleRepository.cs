using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IRoleRepository
    {
        Task<IEnumerable<RoleDto>> AddRole(IEnumerable<RoleDto> role);
        Task<IEnumerable<RoleDto>> UpdateRole(IEnumerable<RoleDto> role);
        Task<RoleDto> GetRoleById(int id);
        Task<RoleDto> GetRoleByName(string name);
        Task<IEnumerable<RoleDto>> GetRoles();

        Task<IEnumerable<RoleDto>> GetFilteredRole();

    }
}
