using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IPermissionRepository
    {
        Task<IEnumerable<PermissionDto>> AddPermission(IEnumerable<PermissionDto> permission);
Task<IEnumerable<PermissionDto>> UpdatePermission(IEnumerable<PermissionDto> permission);
Task<PermissionDto> GetPermissionById(int id);
Task<IEnumerable<PermissionDto>> GetPermissions();
Task<IEnumerable<PermissionDto>> GetPermissionByRoleId(int roleId);
Task<IEnumerable<PermissionDto>> GetPermissionsByUser(int userId);
Task<IEnumerable<PermissionDto>> GetPermissionsByUserMenu(int userId, int menuId);
Task<IEnumerable<PermissionDto>> GetPermissionsByRoleMenu(int roleId, int menuId);
Task<IEnumerable<PermissionDto>> GetPermissionsByRole(int roleId);
Task<IEnumerable<PermissionDto>> GetPermissionsByIds(int[] ids);

    }
}
