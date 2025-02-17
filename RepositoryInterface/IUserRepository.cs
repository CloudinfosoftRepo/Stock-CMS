using System.Linq.Expressions;
using Stock_CMS.Entity;
using Stock_CMS.Models;

namespace Stock_CMS.RepositoryInterface
{
    public interface IUserRepository
    {
        Task<IEnumerable<UserDto>> Login(string userName, string passWord);
        Task<IEnumerable<UserDto>> LoginDetails();
        Task<IEnumerable<UserDto>> GetAllUsers();
        Task<IEnumerable<UserDto>> ForgotPassword(string userName);
        Task<IEnumerable<UserDto>> AddUser(IEnumerable<UserDto> user);

        Task<IEnumerable<UserDto>> AddUsers(IEnumerable<UserDto> user);
        Task<IEnumerable<UserDto>> UpdateUser(IEnumerable<UserDto> user);
        Task<UserDto> GetUserById(int id);
        Task<IEnumerable<UserDto>> GetUserByUserId(long id);
        Task<IEnumerable<UserDto>> GetUserByIds(int[] ids);
        Task<UserDto> GetUserByEmail(string email);
        Task<IEnumerable<UserDto>> GetUsers(Expression<Func<TblUser, bool>> criteria);
        Task<IEnumerable<UserDto>> GetUsersByIds(int?[] ids);

        Task<IEnumerable<UserDto>> GetUserByInfo(UserDto data);

        Task<IEnumerable<UserDto>> UpdateUserbyColumn(IEnumerable<UserDto> data, string[] columns);

    }
}
