using BusinessLayer.Interfaces;
using Common.DAO;
using Common.DTO;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public class UsersBL : IUsersBL
    {
        private IUsersRepo _userRepo;

        public UsersBL(IUsersRepo userRepo)
        {
            this._userRepo = userRepo;
        }

        public bool CreateUser(RegisterDTO user)
        {
            return _userRepo.CreateUser(user);
        }
        public async Task<string> Login(LoginDTO user)
        {
            return await _userRepo.Login(user);
        }
        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            return await _userRepo.GetUserByIdAsync(userId);
        }
        public async Task<bool> IsRegisteredAsync(string email)
        {
            return await _userRepo.IsRegisteredAsync(email);
        }
        public async Task<bool> ResetPassword(string email, ResetPasswordDTO resetPassword)
        {
            return await _userRepo.ResetPassword(email, resetPassword);
        }

        public ForgetPasswordDTO ForgetPassword(string email)
        {
            return _userRepo.ForgetPassword(email);
        }

       
    }
}
