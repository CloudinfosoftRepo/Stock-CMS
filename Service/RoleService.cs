using System.Diagnostics;
using Stock_CMS.Models;
using Stock_CMS.Repository;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;


        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;

        }

        public async Task<string> AddRole(RoleDto role, int flag)
        {
            List<RoleDto> list = new List<RoleDto>();

            if (flag == 0)
            {
                if (role != null)
                {
                    var existingName = await _roleRepository.GetRoleByName(role.Name);
                    if (existingName != null)
                    {
                        return "Same Role Already Exists";
                    }
                    role.IsActive = true;
                    role.CreatedAt = DateTime.Now;
                    list.Add(role);
                    var result = await _roleRepository.AddRole(list);
                    return "Role Added Successfully";
                }
                return "Failed To Add Role";
            }
            else
            {
                var existing = await _roleRepository.GetRoleById(role.Id);
                if (existing != null)
                {
                    var existingName = await _roleRepository.GetRoleByName(role.Name);
                    if (existingName != null && existingName.Id != role.Id)
                    {
                        return "Same Role Already Exists";
                    }
                    RoleDto item = new()
                    {
                        Id = existing.Id,
                        Name = role.Name ?? existing.Name,
                        IsActive = role.IsActive ?? existing.IsActive,
                        CreatedAt = existing.CreatedAt,
                        CreatedBy = existing.CreatedBy,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = role.UpdatedBy,
                    };

                    list.Add(item);
                    var result = await _roleRepository.UpdateRole(list);
                    if (result.First().IsActive == false)
                    {
                        return "Role Deleted";
                    }
                    else if (result != null)
                    {
                        return "Role Updated Successfully";
                    }
                    return "Failed To Update Role";
                }
                return "Role Not Found";
            }
        }


        public async Task<IEnumerable<RoleDto>> GetRole()
        {
            var role = await _roleRepository.GetRoles();
            var result = from r in role
                         select new RoleDto
                         {
                             Id = r.Id,
                             Name = r.Name,
                             IsActive = r.IsActive,
                             CreatedAt = r.CreatedAt,
                             CreatedBy = r.CreatedBy,
                             UpdatedAt = r.UpdatedAt,
                             UpdatedBy = r.UpdatedBy,
                         };
            return result;
        }

    }
}
