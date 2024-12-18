using Common.DAO;
using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IUserRepository
    {
        bool CreateUser(RegisterDTO user);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<string> Login(LoginDTO user);
    }
}