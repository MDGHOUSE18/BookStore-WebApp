﻿using Common.DAO;
using Common.DTO;

namespace DataAccessLayer.Interfaces
{
    public interface IUsersRepo
    {
        bool CreateUser(RegisterDTO user);
        Task<UserDTO> GetUserByIdAsync(int userId);
        Task<bool> IsRegisteredAsync(string email);
        Task<LoginResDTO> Login(LoginDTO user);
        ForgetPasswordDTO ForgetPassword(string email);
        Task<bool> ResetPassword(string email, ResetPasswordDTO resetPassword);
    }
}