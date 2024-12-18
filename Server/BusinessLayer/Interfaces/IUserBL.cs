using Common.DAO;
using Common.DTO;

namespace BusinessLayer.Interfaces
{
    public interface IUserBL
    {
        bool CreateUser(RegisterDTO user);
        Task<string> Login(LoginDTO user);
        Task<UserDTO> GetUserByIdAsync(int userId);
    }
}