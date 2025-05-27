using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IPermissionService
    {
        Task<IEnumerable<MenuDto>> GetMenu();
Task<IEnumerable<PermissionDto>> GetPermissionsByUserMenu(int userId, int menuId);
Task<dynamic> GetMenuByUser(int UserId);
Task<dynamic> GetMenuByRole(int RoleId);
Task<IEnumerable<PermissionDto>> GetPermissionsByRole(int roleId);
Task<IEnumerable<PermissionDto>> GetPermissionsByUserId(int userId);

        Task<dynamic> UpdatePermissionByUserMenu(PermissionDto perm);
Task<dynamic> UpdatePermissionByRoleMenu(PermissionDto perm);


    }
}
