using Common.DTO;
using Common.Modals;
using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace DataAccessLayer.Services
{
    public class CartRepo : ICartRepo
    {
        private readonly string _conString;

        public CartRepo(IConfiguration configuration)
        {
            this._conString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> AddCartItemAsync(AddCartDTO cart,int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddCartItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BookId", cart.BookId);
                cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);
                await con.OpenAsync();
                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
                //SqlDataReader reader = await command.ExecuteReaderAsync();
                //if (await reader.ReadAsync())
                //{
                //    return new CartItemDTO
                //    {
                //        Title = reader["Title"].ToString(),
                //        Author = reader["Author"].ToString(),
                //        Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                //        DiscountedPrice = reader.IsDBNull(reader.GetOrdinal("DiscountedPrice"))
                //            ? (decimal?)null
                //            : reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                //        ImageUrl = reader["ImageUrl"].ToString(),
                //        StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                //    };
                //}
            }
            //return null;
        }

        public async Task<bool> DeleteCartAsync(int cartId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteCart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@CartID", cartId);

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<CartItemDTO>> GetCartItemsAsync(int userId)
        {
            var cartItems = new List<CartItemDTO>();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetCartItems", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.AddWithValue("@UserID", userId);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var cartItem = new CartItemDTO
                        {
                            Title = reader["Title"].ToString(),
                            Author = reader["Author"].ToString(),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            DiscountedPrice = reader.IsDBNull(reader.GetOrdinal("DiscountedPrice"))
                                ? (decimal?)null
                                : reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                            ImageUrl = reader["ImageUrl"].ToString(),
                            StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                        };

                        cartItems.Add(cartItem);
                    }
                }
            }

            return cartItems;
        }


        public async Task<bool> RemoveAllCartItemsAsync(int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_RemoveAllFromCart", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

    }
}
