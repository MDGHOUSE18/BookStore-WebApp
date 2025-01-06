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

        //public async Task<List<OrderDTO>> GetOrdersAsync(int userId)
        //{
        //    using (SqlConnection con = new SqlConnection(_conString))
        //    {
        //        SqlCommand cmd = new SqlCommand("usp_GetOrders", con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@UserId", userId);

        //        List<OrderDTO> orders = new List<OrderDTO>();

        //        await con.OpenAsync();
        //        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
        //        {
        //            while (await reader.ReadAsync())
        //            {
        //                orders.Add(new OrderDTO
        //                {
        //                    OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
        //                    TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
        //                    TotalDiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
        //                    OrderStatus = reader["Status"].ToString(),
        //                    OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
        //                    Image = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
        //                    Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
        //                    BookTitle = reader["BookTitle"].ToString(),
        //                    Author = reader["Author"].ToString()
        //                });
        //            }
        //        }

        //        return orders;
        //    }
        //}
        public async Task<List<OrderDTO>> GetOrdersAsync(int userId)
        {
            try
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
                                TotalDiscountedPrice = reader.GetDecimal(reader.GetOrdinal("TotalDiscountedPrice")),
                                OrderStatus = reader["Status"].ToString(),
                                OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                                Image = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
                                Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                                BookTitle = reader["BookTitle"].ToString(),
                                Author = reader["Author"].ToString()
                            });
                        }
                    }

                    return orders;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error retrieving orders: " + ex.Message);
                throw;
            }
        }


        public async Task<OrderDTO> GetOrderAsync(int orderId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);

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

        public async Task<OrderDetailDTO> GetOrderDetailsAsync(int orderId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@OrderId", orderId);

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new OrderDetailDTO
                        {
                            OrderId = reader.GetInt32(reader.GetOrdinal("OrderId")),
                            BookTitle = reader.GetString(reader.GetOrdinal("BookTitle")),
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            Image = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
                            Quantity = reader.GetInt32(reader.GetOrdinal("Quantity")),
                            TotalPrice = reader.GetDecimal(reader.GetOrdinal("TotalPrice")),
                            DiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                            OrderDate = reader.GetDateTime(reader.GetOrdinal("OrderDate")),
                            OrderStatus = reader["Status"].ToString()
                        };
                    }
                }

                return null;
            }
        }

    }
}

