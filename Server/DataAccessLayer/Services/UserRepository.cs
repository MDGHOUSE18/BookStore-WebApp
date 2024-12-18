using Common.DAO;
using Common.DTO;
using DataAccessLayer.DataAccess;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SqlHelper _sqlHelper;
        private readonly TokenHelper _tokenHelper;

        public UserRepository(SqlHelper sqlHelper, TokenHelper tokenHelper)
        {
            _sqlHelper = sqlHelper;
            _tokenHelper = tokenHelper;
        }

        // Add a new user using stored procedure
        public bool CreateUser(RegisterDTO user)
        {

            user.PasswordHash = EncodePasswordToBase64(user.PasswordHash);
            Console.WriteLine(user.FullName);
            var query = "CreateUser";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@FullName", user.FullName),
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@PasswordHash", user.PasswordHash),
                new SqlParameter("@Role", "user"),
                new SqlParameter("@PhoneNumber", user.Phone)
            };
            
            int rowsAffected = _sqlHelper.ExecuteNonQueryAsync(query, parameters).Result;
            return rowsAffected > 0;
        }

        private static string EncodePasswordToBase64(string password)
        {
            try
            {
                byte[] encData_byte = new byte[password.Length];
                encData_byte = System.Text.Encoding.UTF8.GetBytes(password);
                string encodedData = Convert.ToBase64String(encData_byte);
                return encodedData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode" + ex.Message);
            }
        }

        public async Task<string> Login(LoginDTO user)
        {
            user.Password = EncodePasswordToBase64(user.Password);
            var query = "Login";
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Email", user.Email),
                new SqlParameter("@Password", user.Password)
            };

            var dataTable = await _sqlHelper.ExecuteStoredProcedureAsync(query, parameters);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            int Id = Convert.ToInt32(row["UserId"]);

            return _tokenHelper.GenerateJwtToken(Id, user.Email);
            //return new UserDTO
            //{
            //    //Id = Convert.ToInt32(row["UserId"]),
            //    FullName = row["FullName"].ToString(),
            //    Email = row["Email"].ToString(),
            //    Role = row["Role"].ToString(),
            //    Address = row["Address"].ToString(),
            //    PhoneNumber = row["PhoneNumber"].ToString(),
            //    CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
            //    //UpdatedAt = row["UpdatedAt"] == DBNull.Value ? null : Convert.ToDateTime(row["UpdatedAt"]),
            //    //LastLogin = row["LastLogin"] == DBNull.Value ? null : Convert.ToDateTime(row["LastLogin"]),
            //    //IsActive = Convert.ToBoolean(row["IsActive"])
            //};
        }

        public async Task<UserDTO> GetUserByIdAsync(int userId)
        {
            var query = "GetUserById"; // Stored Procedure Name
            var parameters = new SqlParameter[]
            {
                new SqlParameter("@Id", userId)
            };

            var dataTable = await _sqlHelper.ExecuteStoredProcedureAsync(query, parameters);

            if (dataTable.Rows.Count == 0) return null;

            var row = dataTable.Rows[0];
            return new UserDTO
            {
                //Id = Convert.ToInt32(row["Id"]),
                FullName = row["FullName"].ToString(),
                Email = row["Email"].ToString(),
                Role = row["Role"].ToString(),
                Address = row["Address"].ToString(),
                PhoneNumber = row["PhoneNumber"].ToString(),
                CreatedAt = Convert.ToDateTime(row["CreatedAt"]),
                //UpdatedAt = row["UpdatedAt"] == DBNull.Value ? null : Convert.ToDateTime(row["UpdatedAt"]),
                //LastLogin = row["LastLogin"] == DBNull.Value ? null : Convert.ToDateTime(row["LastLogin"]),
                //IsActive = Convert.ToBoolean(row["IsActive"])
            };
        }


    }
}
