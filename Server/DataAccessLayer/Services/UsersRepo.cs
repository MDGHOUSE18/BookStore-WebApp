using Common.DAO;
using Common.DTO;
using Common.Modals;
using DataAccessLayer.Helpers;
using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Repositories
{
    public class UsersRepo : IUsersRepo
    {
        private readonly string _connectionString;
        private readonly TokenHelper _tokenHelper;


        public UsersRepo(IConfiguration configuration,TokenHelper tokenHelper)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _tokenHelper = tokenHelper;
        }

        public Task<bool> IsRegisteredAsync(string email)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_CheckUserByEmail", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email);

                con.Open();
                int result = Convert.ToInt32(cmd.ExecuteScalar());
                return Task.FromResult(result > 0);
            }
        }

        public bool CreateUser(RegisterDTO user)
        {
            user.PasswordHash = EncodePasswordToBase64(user.PasswordHash);
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_CreateUser", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(
                    new SqlParameter[]
                        {
                            new SqlParameter("@FullName", user.FullName),
                            new SqlParameter("@Email", user.Email),
                            new SqlParameter("@PasswordHash", user.PasswordHash),
                            new SqlParameter("@Role", "User"),
                            new SqlParameter("@PhoneNumber", user.Phone)
                        }
                    );

                con.Open();
                int rowsEffected = cmd.ExecuteNonQuery();

                return rowsEffected>0;
            }
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

        public Task<string> Login(LoginDTO user)
        {
            user.Password = EncodePasswordToBase64(user.Password);

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_Login", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(
                    new SqlParameter[]
                        {
                            new SqlParameter("@Email", user.Email),
                            new SqlParameter("@Password", user.Password)
                        }
                    );

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    int Id = Convert.ToInt32(reader["UserId"]);
                    var token = _tokenHelper.GenerateJwtToken(Id, user.Email);
                    return Task.FromResult(token);
                }

                return Task.FromResult<string>(null);
            }

        }

        public ForgetPasswordDTO ForgetPassword(string email)
        {
            ForgetPasswordDTO forgetPassword = new ForgetPasswordDTO();
            forgetPassword.Email = email;
            forgetPassword.Token = _tokenHelper.GenerateResetPasswordToken(email); 
            return forgetPassword;
        }

        public Task<UserDTO> GetUserByIdAsync(int userId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetUserById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Id", userId);

                con.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.Read())
                {
                    return Task.FromResult(new UserDTO
                    {
                        FullName = reader["FullName"].ToString(),
                        Email = reader["Email"].ToString(),
                        Role = reader["Role"].ToString(),
                        PhoneNumber = reader["PhoneNumber"].ToString(),
                        CreatedAt = Convert.ToDateTime(reader["CreatedAt"])
                    });
                }

                return Task.FromResult<UserDTO>(null);
            }
        }

        public Task<bool> ResetPassword(string email, ResetPasswordDTO resetPassword)
        {
            string PasswordHash = EncodePasswordToBase64(resetPassword.Password);


            using(SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_ResetPassword", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(
                    new SqlParameter[]
                        {
                        new SqlParameter("@Email", email),
                        new SqlParameter("@Password", PasswordHash)
                        }
                    );

                con.Open();
                int row = cmd.ExecuteNonQuery();
                return Task.FromResult(row > 0);
            }
            
        }

    }
}
