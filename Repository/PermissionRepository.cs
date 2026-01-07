using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
    public class PermissionRepository : EfRepository<TblPermission, PermissionDto>, IPermissionRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;

        public PermissionRepository(DmCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PermissionDto>> AddPermission(IEnumerable<PermissionDto> permission)
        {
            return await AddEntities(permission);
        }
        public async Task<IEnumerable<PermissionDto>> UpdatePermission(IEnumerable<PermissionDto> permission)
        {
            return await UpdateEntities(permission);
        }
        public async Task<PermissionDto> GetPermissionById(int id)
        {
            return await GetOne(x => x.Id == id);
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissions()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionByRoleId(int roleId)
        {
            var permissions = await GetMany(x => x.IsActive == true && x.RoleId == roleId);

            var maxUserId = permissions
                .Min(p => p.UserId);

            // Now get the permissions for that user
            var userPermissions = permissions
                .Where(p => p.UserId == maxUserId)
                .ToList();

            return userPermissions;
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByUser(int userId)
        {
            return await GetMany(x => x.IsActive == true && x.UserId == userId);
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByUserMenu(int userId, int menuId)
        {
            return await GetMany(x => x.IsActive == true && x.UserId == userId && x.MenuId == menuId);
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByRoleMenu(int roleId, int menuId)
        {
            return await GetMany(x => x.IsActive == true && x.RoleId == roleId && x.MenuId == menuId);
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByRole(int roleId)
        {
            return await GetMany(x => x.IsActive == true && x.RoleId == roleId);
        }
        public async Task<IEnumerable<PermissionDto>> GetPermissionsByIds(int[] ids)
        {
            return await GetMany(x => ids.Contains(x.Id));
        }
    }
}
