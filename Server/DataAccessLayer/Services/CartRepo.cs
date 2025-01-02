using Common.DTO;
using Common.Modals;
using DataAccessLayer.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace DataAccessLayer.Services
{
    public class CartRepo : ICartRepo
    {
        private readonly string _conString;

        public CartRepo(IConfiguration configuration)
        {
            this._conString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<CartItemDTO> AddCartItemAsync(AddCartDTO cart, int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddCartItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BookId", cart.BookId);
                cmd.Parameters.AddWithValue("@Quantity", cart.Quantity);

                await con.OpenAsync();

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        return new CartItemDTO
                        {
                            CartId = reader.GetInt32(reader.GetOrdinal("CartId")),
                            BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                            Title = reader["Title"].ToString(),
                            Author = reader["Author"].ToString(),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            DiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                            //ImageUrl = reader["ImageUrl"].ToString(),
                            CartQuantity = reader.GetInt32(reader.GetOrdinal("CartQuantity"))
                        };
                    }
                }
            }

            return null;
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
                            CartId = reader.GetInt32(reader.GetOrdinal("CartId")),
                            BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                            Title = reader["Title"].ToString(),
                            Author = reader["Author"].ToString(),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            DiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                            //ImageUrl = reader["ImageUrl"].ToString(),
                            ImageUrl = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
                            CartQuantity = reader.GetInt32(reader.GetOrdinal("CartQuantity")),
                            StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))
                        };

                        cartItems.Add(cartItem);
                    }
                }
            }

            return cartItems;
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
        public async Task<bool> UpdateCartItemQuantityAsync(UpdateCartItemDTO dto, int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateCartItemQuantity", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BookId", dto.BookId);
                cmd.Parameters.AddWithValue("@NewQuantity", dto.NewQuantity);  

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
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
