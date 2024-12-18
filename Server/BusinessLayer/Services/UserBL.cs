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
    public class UserBL : IUserBL
    {
        private IUserRepository _userRepo;

        public UserBL(IUserRepository userRepo)
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
    }
}
