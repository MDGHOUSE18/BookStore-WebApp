using Common.DAO;
using Common.DTO;

namespace BusinessLayer.Interfaces
{
    public interface IUsersBL
    {
        bool CreateUser(RegisterDTO user);
        Task<LoginResDTO> Login(LoginDTO user);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<bool> IsRegisteredAsync(string email);
        ForgetPasswordDTO ForgetPassword(string email);

        Task<bool> ResetPassword(string email, ResetPasswordDTO resetPassword);
    }
}