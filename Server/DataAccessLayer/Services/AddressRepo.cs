using Common.DTO;
using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class AddressRepo : IAddressRepo
    {
        private readonly string _conString;

        public AddressRepo(IConfiguration configuration)
        {
            this._conString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<AddressDTO> AddAddressAsync(AddAddressDTO address, int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@TypeID", address.TypeId);
                cmd.Parameters.AddWithValue("@Address", address.Address);
                cmd.Parameters.AddWithValue("@City", address.City);
                cmd.Parameters.AddWithValue("@State", address.State);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new AddressDTO
                        {
                            FullName = reader.GetString("FullName"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            TypeOfAddress = reader.GetString("TypeOfAddress"),
                            Address = reader.GetString("Address"),
                            City = reader.GetString("City"),
                            State = reader.GetString("State")
                        };
                    }
                    return null;
                }
            }
        }

        public async Task<AddressDTO> GetAddressByIdAsync(int addressId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAddressById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AddressID", addressId);

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new AddressDTO
                        {
                            FullName = reader.GetString("FullName"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            TypeOfAddress = reader.GetString("TypeOfAddress"),
                            Address = reader.GetString("Address"),
                            City = reader.GetString("City"),
                            State = reader.GetString("State")
                        };
                    }
                    return null;
                }
            }
        }

        public async Task<AddressDTO> UpdateAddressAsync(UpdateAddressDTO address,int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AddressID", address.AddressId);
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@TypeID", address.TypeId);
                cmd.Parameters.AddWithValue("@Address", address.Address);
                cmd.Parameters.AddWithValue("@City", address.City);
                cmd.Parameters.AddWithValue("@State", address.State);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new AddressDTO
                        {
                            FullName = reader.GetString("FullName"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            TypeOfAddress = reader.GetString("TypeOfAddress"),
                            Address = reader.GetString("Address"),
                            City = reader.GetString("City"),
                            State = reader.GetString("State")
                        };
                    }
                    return null;
                }
            }
        }

        public async Task<bool> DeleteAddressAsync(int addressId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteAddress", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AddressID", addressId);

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<AddressDTO>> GetAddressesByUserIdAsync(int userId)
        {
            List<AddressDTO> addresses = new List<AddressDTO>();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAddressesByUserId", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        addresses.Add(new AddressDTO
                        {
                            FullName = reader.GetString("FullName"),
                            PhoneNumber = reader.GetString("PhoneNumber"),
                            TypeOfAddress = reader.GetString("TypeOfAddress"),
                            Address = reader.GetString("Address"),
                            City = reader.GetString("City"),
                            State = reader.GetString("State")
                        });
                    }
                }
            }

            return addresses;
        }


    }
}
