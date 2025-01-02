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
    public class WishListRepo : IWishListRepo
    {
        private readonly string _conString;

        public WishListRepo(IConfiguration configuration)
        {
            _conString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<bool> AddWishListAsync(int userId, int bookId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddWishList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BookId", bookId);

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<List<WishListItemDTO>> GetWishListAsync(int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetWishList", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);

                await con.OpenAsync();

                List<WishListItemDTO> wishList = new List<WishListItemDTO>();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        wishList.Add(new WishListItemDTO
                        {
                            WishListId= reader.GetInt32(reader.GetOrdinal("WishListId")),
                            BookId = reader.GetInt32(reader.GetOrdinal("BookId")),
                            Title = reader.GetString(reader.GetOrdinal("Title")),
                            Author = reader.GetString(reader.GetOrdinal("Author")),
                            Price = reader.GetDecimal(reader.GetOrdinal("Price")),
                            DiscountedPrice = reader.GetDecimal(reader.GetOrdinal("DiscountedPrice")),
                            //ImageUrl = reader.GetString(reader.GetOrdinal("ImageData")),
                            ImageUrl = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
                            StockQuantity = reader.GetInt32(reader.GetOrdinal("StockQuantity"))

                        });
                    }
                }

                return wishList;
            }
        }

        public async Task<bool> DeleteWishListItemAsync(int wishListId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteItem", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@WishListId", wishListId);

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
        }

        public async Task<bool> RemoveAllWishListItemsAsync(int userId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_RemoveAll", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                await con.OpenAsync();

                int result = await cmd.ExecuteNonQueryAsync();
                return result > 0;
            }
        }
    }

}
