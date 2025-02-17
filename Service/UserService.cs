using Stock_CMS.Common;
using Stock_CMS.Models;
using Stock_CMS.Repository;
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

        public async Task<IEnumerable<UserDto>> GetUserByRole()
        {
            var user = await _user.GetUsers(x => x.IsActive == true);
            var role = await _roleRepository.GetRoles();

            var data = user.Where(x => x.RoleId != 1).ToList();

            var result = from u in data
                         join r in role on u.RoleId equals r.Id into rGroup
                         from r in rGroup.DefaultIfEmpty()
                         select new UserDto
                         {
                             Id = u.Id,
                             Name = u.Name,
                             RoleId = u.RoleId,
                             RoleName = r.Name,
                             Email = u.Email,
                             Password = u.Password,
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

        public async Task<int> AddUsers(UserDto data)
        {
            var isExist = await _user.GetUserByInfo(data);
            if (isExist.Any()) { return -1; }
            else
            {
                List<UserDto> dataList = new List<UserDto> { data };
                var result = await _user.AddUsers(dataList);
                if (result.Any())
                {
                    return result.FirstOrDefault().Id;
                }
                else
                {
                    return 0;
                }
            }
        }

        public async Task<Int32> UpdateUsers(UserDto data)
        {
            var isExist = await _user.GetUserByUserId(data.Id);
            var chk = await _user.GetUserByInfo(data);

            bool isMatch = chk.Any(x => x.Name.ToLower() == data.Name.ToLower() && x.Id != data.Id && (x.Email == data.Email && x.ContactNo == data.ContactNo));
            if (isMatch)
            {
                return -1;
            }

            if (isExist.Any())
            {
                var existingProduct = isExist.FirstOrDefault();
                if (existingProduct != null)
                {
                    data.CreatedBy = existingProduct.CreatedBy;
                    data.CreatedAt = existingProduct.CreatedAt;
                    data.UpdatedBy = data.UpdatedBy;
                    data.UpdatedAt = DateTime.Now;
                    data.IsActive = existingProduct.IsActive;
                }

                List<UserDto> updateList = new List<UserDto> { data };
                var result = await _user.UpdateUser(updateList);
                if (result.Any())
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return -2;
            }

        }

        public async Task<long> UpdateUserbyColumn(UserDto data)
        {
            data.IsActive = false;
            List<UserDto> updatelist = new List<UserDto>() { data };
            var result = await _user.UpdateUserbyColumn(updatelist, ["IsActive", "UpdatedAt", "UpdatedBy"]);
            if (result.Any())
            {
                return result.FirstOrDefault().Id;
            }
            else
            {
                return 0;
            }
        }
    }
}
