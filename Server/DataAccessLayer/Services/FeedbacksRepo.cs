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
    public class FeedbacksRepo : IFeedbacksRepo
    {
        private readonly string _connectionString;

        public FeedbacksRepo(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Defaultconnection");
        }

        public async Task<FeedbackDTO> AddFeedbackAsync(AddFeedbackDTO feedbackDTO, int userId)
        {

            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_AddFeedback", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserId", userId);
                cmd.Parameters.AddWithValue("@BookId", feedbackDTO.BookId);
                cmd.Parameters.AddWithValue("@Rating", feedbackDTO.Rating);
                cmd.Parameters.AddWithValue("@Comments", feedbackDTO.Comments);

                await con.OpenAsync();
                SqlDataReader reader = await cmd.ExecuteReaderAsync();
                if (reader.Read())
                {
                    return new FeedbackDTO
                    {
                        FeedbackId = Convert.ToInt32(reader["FeedbackId"]),
                        BookId = Convert.ToInt32(reader["BookId"]),
                        Rating = Convert.ToDecimal(reader["Rating"]),
                        Comments = reader["Comments"].ToString()
                    };
                }

                return null;
            }
        }

        public async Task<List<FeedbackDTO>> GetFeedbacksAsync(int bookId)
        {
            using (SqlConnection con = new SqlConnection(_connectionString))
            {
                SqlCommand cmd = new SqlCommand("usp_GetFeedbacksForBook", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BookId", bookId);

                List<FeedbackDTO> feedbacks = new List<FeedbackDTO>();

                await con.OpenAsync();
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        feedbacks.Add(new FeedbackDTO
                        {
                            FeedbackId = Convert.ToInt32(reader["FeedbackId"]),
                            BookId = Convert.ToInt32(reader["BookId"]),
                            Rating = Convert.ToDecimal(reader["Rating"]),
                            Comments = reader["Comments"].ToString()
                        });
                    }
                }

                return feedbacks;
            }
        }
    }
}
