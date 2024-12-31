using Stock_CMS.Models;
using Stock_CMS.RepositoryInterface;
using Stock_CMS.ServiceInterface;

namespace Stock_CMS.Service
{
    public class UserService : IUserService
    {
        public readonly IUserRepository _user;
        public readonly IRoleRepository _roleRepository;
        public UserService(IUserRepository user, IRoleRepository roleRepository)
        {
            _user = user;
            _roleRepository = roleRepository;
        }
        public async Task<IEnumerable<UserDto>> Login(string userName, string passWord)
        {
            return await _user.Login(userName, passWord);
        }
        public async Task<IEnumerable<UserDto>> ForgotPassword(string user)
        {
            return await _user.ForgotPassword(user);
        }
        public async Task<string> AddUser(UserDto user, int flag)
        {

            List<UserDto> list = new List<UserDto>();

            if (flag == 0)
            {
                var existingName = await _user.GetUserByEmail(user.Email);
                if (existingName != null && existingName.Id != user.Id)
                {
                    return "User Already Exists with same Email.";
                }
                if (user != null)
                {
                    user.IsActive = true;
                    user.CreatedAt = DateTime.Now;
                    list.Add(user);
                    var result = await _user.AddUser(list);
                    return "User Added Successfully";
                }
                return "Failed To Add User";

            }
            else
            {
                var existing = await _user.GetUserById(user.Id);
                var existingName = await _user.GetUserByEmail(user.Email);
                if (existingName != null && existingName.Id != user.Id)
                {
                    return "User Already Exists with same Email.";
                }
                if (existing != null)
                {
                    UserDto item = new()
                    {
                        Id = existing.Id,
                        Name = user.Name ?? existing.Name,
                        RoleId = user.RoleId != 0 ? user.RoleId : existing.RoleId,
                        Address = user.Address ?? existing.Address,
                        Password = user.Password ?? existing.Password,
                        Email = user.Email ?? existing.Email,
                        ContactNo = user.ContactNo.HasValue ? user.ContactNo : existing.ContactNo,
                        IsActive = user.IsActive ?? existing.IsActive,
                        CreatedAt = existing.CreatedAt,
                        CreatedBy = existing.CreatedBy,
                        UpdatedAt = DateTime.Now,
                        UpdatedBy = user.UpdatedBy,
                    };

                    list.Add(item);
                    var result = await _user.UpdateUser(list);
                    if (result.First().IsActive == false)
                    {
                        return "User Deleted";
                    }
                    else if (result != null)
                    {
                        return "User Updated Successfully";
                    }
                    return "Failed To Update User";
                }
                return "User Not Found";
            }
        }

        public async Task<IEnumerable<UserDto>> GetUser(bool status)
        {
            var user = await _user.GetUsers(x => x.IsActive == status);
            var role = await _roleRepository.GetRoles();
            var result = from u in user
                         join r in role on u.RoleId equals r.Id into rGroup
                         from r in rGroup.DefaultIfEmpty()
                         select new UserDto
                         {
                             Id = u.Id,
                             Name = u.Name,
                             RoleId = u.RoleId,
                             RoleName = r.Name,
                             Email = u.Email,
                             //  Password = u.Password,
                             Address = u.Address,
                             ContactNo = u.ContactNo,
                             IsActive = u.IsActive,
                             CreatedAt = u.CreatedAt,
                             CreatedBy = u.CreatedBy,
                             UpdatedAt = u.UpdatedAt,
                             UpdatedBy = u.UpdatedBy,
                         };
            return result;
        }
        public async Task<UserDto> GetUserById(int id)
        {
            var user = await _user.GetUserById(id);
            var role = await _roleRepository.GetRoleById(user.RoleId);
            var result = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                RoleId = user.RoleId,
                RoleName = role.Name,
                Email = user.Email,
                Password = user.Password,
                Address = user.Address,
                ContactNo = user.ContactNo,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
                CreatedBy = user.CreatedBy,
                UpdatedAt = user.UpdatedAt,
                UpdatedBy = user.UpdatedBy,
            };
            return result;
        }
    }
}
