using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class FeedbackRepository
    {
        private readonly IConfiguration _configuration;

        public FeedbackRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all feedbacks
        public List<FeedbackModel> GetAllFeedbacks()
        {
            string connectionString = GetConnectionString();
            List<FeedbackModel> feedbacks = new List<FeedbackModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Feedback_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            FeedbackModel feedback = new FeedbackModel
                            {
                                FeedbackId = reader.GetInt32(reader.GetOrdinal("feedback_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                LessonId = reader.IsDBNull(reader.GetOrdinal("lesson_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                Rating = reader.GetInt32(reader.GetOrdinal("rating")),
                                Comment = reader.IsDBNull(reader.GetOrdinal("comment")) ? null : reader.GetString(reader.GetOrdinal("comment")),
                                FeedbackDate = reader.GetDateTime(reader.GetOrdinal("feedback_date"))
                            };
                            feedbacks.Add(feedback);
                        }
                    }
                }
            }
            return feedbacks;
        }

        // Get feedback by ID
        public FeedbackModel GetFeedbackById(int feedbackId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Feedback_SelectById";
                    command.Parameters.AddWithValue("@feedback_id", feedbackId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new FeedbackModel
                            {
                                FeedbackId = reader.GetInt32(reader.GetOrdinal("feedback_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                LessonId = reader.IsDBNull(reader.GetOrdinal("lesson_id")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                Rating = reader.GetInt32(reader.GetOrdinal("rating")),
                                Comment = reader.IsDBNull(reader.GetOrdinal("comment")) ? null : reader.GetString(reader.GetOrdinal("comment")),
                                FeedbackDate = reader.GetDateTime(reader.GetOrdinal("feedback_date"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert feedback
        public bool InsertFeedback(FeedbackModel feedback)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Feedback_Insert";
                    command.Parameters.AddWithValue("@user_id", feedback.UserId);
                    command.Parameters.AddWithValue("@lesson_id", (object)feedback.LessonId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@rating", feedback.Rating);
                    command.Parameters.AddWithValue("@comment", (object)feedback.Comment ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update feedback
        public bool UpdateFeedback(FeedbackModel feedback)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Feedback_Update";
                    command.Parameters.AddWithValue("@feedback_id", feedback.FeedbackId);
                    command.Parameters.AddWithValue("@lesson_id", (object)feedback.LessonId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@rating", feedback.Rating);
                    command.Parameters.AddWithValue("@comment", (object)feedback.Comment ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete feedback
        public bool DeleteFeedback(int feedbackId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Feedback_Delete";
                    command.Parameters.AddWithValue("@feedback_id", feedbackId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
