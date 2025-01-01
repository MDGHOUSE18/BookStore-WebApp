using Common.DTO;
using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class OrdersRepo : IOrdersRepo
    {
        private readonly string _conString;

        public OrdersRepo(IConfiguration configuration)
        {
            _conString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<OrderDTO> AddOrderAsync(int userId, int addressId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_CreateOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@AddressID", addressId);

                await con.OpenAsync();

                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    return new OrderDTO
                    {
                        OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                        TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                        TotalDiscountedPrice = reader.GetDecimal(reader.GetOrdinal("TotalDiscountedPrice")),
                        OrderStatus = reader["Status"].ToString(),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
                    };
                }
                return null;
            }
        }

        public async Task<List<OrderDTO>> GetOrdersAsync(int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetOrders", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                List<OrderDTO> orders = new List<OrderDTO>();

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        orders.Add(new OrderDTO
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                            TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                            TotalDiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                            OrderStatus = reader["Status"].ToString(),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
                        });
                    }
                }

                return orders;
            }
        }

        public async Task<OrderDTO> GetOrderAsync(int orderId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                //OrderDTO order = null;

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new OrderDTO
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                            TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                            TotalDiscountedPrice = reader.GetDecimal(reader.GetOrdinal("TotalDiscountedPrice")),
                            OrderStatus = reader["Status"].ToString(),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate"))
                        };
                    }
                }

                return null;
            }
        }

        public async Task<OrderDTO> UpdateOrderStatusAsync(int orderId, int statusId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateOrderStatus", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);
                cmd.Parameters.AddWithValue("@StatusId", statusId);

                await con.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
            }
            return await GetOrderAsync(orderId);
        }
    }
}
