using Common.DTO;
using Common.DTO.Books;
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
    public class BooksRepo : IBooksRepo
    {
        private readonly string _conString;

        public BooksRepo(IConfiguration configuration)
        {
            this._conString = configuration.GetConnectionString("DefaultConnection");
        }


        public async Task<BookDTO> AddBookAsync(AddBookDTO bookDTO)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddBook", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Title", bookDTO.Title);
                cmd.Parameters.AddWithValue("@Author", bookDTO.Author);
                cmd.Parameters.AddWithValue("@Description", bookDTO.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", bookDTO.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", bookDTO.DiscountedPrice ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@ImageUrl", bookDTO.ImageUrl ?? (object)DBNull.Value);
                byte[] imageBytes = string.IsNullOrEmpty(bookDTO.ImageUrl) ? null : Convert.FromBase64String(bookDTO.ImageUrl);
                cmd.Parameters.AddWithValue("@ImageData", imageBytes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StockQuantity", bookDTO.StockQuantity);

                SqlParameter outputBookIdParam = new SqlParameter
                {
                    ParameterName = "@BookID",
                    SqlDbType = SqlDbType.Int,
                    Direction = ParameterDirection.Output
                };
                cmd.Parameters.Add(outputBookIdParam);

                await con.OpenAsync();

                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    int bookId = (int)outputBookIdParam.Value;
                    return new BookDTO
                    {
                        BookID = bookId,
                        Title = bookDTO.Title,
                        Author = bookDTO.Author,
                        Description = bookDTO.Description,
                        Price = bookDTO.Price,
                        DiscountedPrice = bookDTO.DiscountedPrice,
                        ImageUrl = bookDTO.ImageUrl,
                        StockQuantity = bookDTO.StockQuantity,
                        DateAdded = DateTime.Now,
                        LastUpdated = DateTime.Now
                    };
                }

                return null;
            }
        }
        public async Task<BookDTO> GetBookByIdAsync(int bookId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetBookById", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookID", bookId);

                await con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                if (await reader.ReadAsync())
                {
                    return new BookDTO
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        DiscountedPrice = reader["DiscountedPrice"] as decimal?,
                        ImageUrl = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
                        StockQuantity = (int)reader["StockQuantity"],
                        DateAdded = (DateTime)reader["DateAdded"],
                        LastUpdated = (DateTime)reader["LastUpdated"]
                    };
                }

                return null;
            }
        }
        public async Task<BookDTO> UpdateBookAsync(int bookId, AddBookDTO bookDTO)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_UpdateBook", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@BookID", bookId);
                cmd.Parameters.AddWithValue("@Title", bookDTO.Title);
                cmd.Parameters.AddWithValue("@Author", bookDTO.Author);
                cmd.Parameters.AddWithValue("@Description", bookDTO.Description ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Price", bookDTO.Price);
                cmd.Parameters.AddWithValue("@DiscountedPrice", bookDTO.DiscountedPrice ?? (object)DBNull.Value);
                //cmd.Parameters.AddWithValue("@ImageUrl", bookDTO.ImageUrl ?? (object)DBNull.Value);
                byte[] imageBytes = string.IsNullOrEmpty(bookDTO.ImageUrl) ? null : Convert.FromBase64String(bookDTO.ImageUrl);
                cmd.Parameters.AddWithValue("@ImageData", imageBytes ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@StockQuantity", bookDTO.StockQuantity);

                await con.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                if (rowsAffected > 0)
                {
                    return await GetBookByIdAsync(bookId);
                }

                return null;
            }
        }

        public async Task<bool> DeleteBookAsync(int bookId)
        {
            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_DeleteBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookID", bookId);

                await con.OpenAsync();
                int rowsAffected = await cmd.ExecuteNonQueryAsync();

                return rowsAffected > 0;
            }
        }

        public async Task<List<BookDTO>> GetAllBooksAsync()
        {
            List<BookDTO> books = new List<BookDTO>();

            using (SqlConnection con = new SqlConnection(_conString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetAllBooks", con);
                cmd.CommandType = CommandType.StoredProcedure;

                await con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    books.Add(new BookDTO
                    {
                        BookID = (int)reader["BookID"],
                        Title = reader["Title"].ToString(),
                        Author = reader["Author"].ToString(),
                        Description = reader["Description"].ToString(),
                        Price = (decimal)reader["Price"],
                        DiscountedPrice = reader["DiscountedPrice"] as decimal?,
                        ImageUrl = reader["ImageData"] != DBNull.Value ? Convert.ToBase64String((byte[])reader["ImageData"]) : null,
                        StockQuantity = (int)reader["StockQuantity"],
                        DateAdded = (DateTime)reader["DateAdded"],
                        LastUpdated = (DateTime)reader["LastUpdated"]
                    });
                }
            }

            return books;
        }


    }

}
