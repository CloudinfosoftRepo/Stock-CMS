using AutoMapper;
using Stock_CMS.Common;
using Stock_CMS.Entity;
using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;

namespace Stock_CMS.Repository
{
	public class RoleRepository : EfRepository<TblRole, RoleDto>, IRoleRepository
    {
        private readonly DmCmsContext _dbContext;
        private readonly IMapper _mapper;

        public RoleRepository(DmCmsContext dbContext, IMapper mapper) : base(dbContext, mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<IEnumerable<RoleDto>> AddRole(IEnumerable<RoleDto> role)
        {
            return await AddEntities(role);
        }
        public async Task<IEnumerable<RoleDto>> UpdateRole(IEnumerable<RoleDto> role)
        {
            return await UpdateEntities(role);
        }
        public async Task<RoleDto> GetRoleById(int id)
        {
            return await GetOne(x => x.IsActive == true && x.Id == id);
        }
        public async Task<RoleDto> GetRoleByName(string name)
        {
            return await GetOne(x => x.IsActive == true && x.Name == name);
        }
        public async Task<IEnumerable<RoleDto>> GetRoles()
        {
            return await GetMany(x => x.IsActive == true);
        }
        public async Task<IEnumerable<RoleDto>> GetFilteredRole()
        {
            return await GetMany(x => x.IsActive == true && x.Name != "Admin");
        }
    }
}
