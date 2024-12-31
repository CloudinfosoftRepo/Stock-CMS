using Stock_CMS.Models;

namespace Stock_CMS.ServiceInterface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> Login(string userName, string passWord);
        Task<IEnumerable<UserDto>> ForgotPassword(string user);
        Task<string> AddUser(UserDto user, int flag);
        Task<IEnumerable<UserDto>> GetUser(bool status);
        Task<UserDto> GetUserById(int id);

    }
}
