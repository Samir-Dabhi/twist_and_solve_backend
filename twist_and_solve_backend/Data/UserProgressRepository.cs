using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class UserProgressRepository
    {
        private readonly IConfiguration _configuration;

        public UserProgressRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all user progress
        public List<UserProgressModel> GetAllUserProgress()
        {
            string connectionString = GetConnectionString();
            List<UserProgressModel> userProgressList = new List<UserProgressModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Progress_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProgressModel progress = new UserProgressModel
                            {
                                ProgressId = reader.GetInt32(reader.GetOrdinal("progress_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                Completed = reader.IsDBNull(reader.GetOrdinal("completed")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("completed")),
                                CompletionDate = reader.IsDBNull(reader.GetOrdinal("completion_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("completion_date"))
                            };
                            userProgressList.Add(progress);
                        }
                    }
                }
            }
            return userProgressList;
        }

        // Get progress by user ID
        public List<UserProgressModel> GetUserProgressByUserId(int userId)
        {
            string connectionString = GetConnectionString();
            List<UserProgressModel> userProgressList = new List<UserProgressModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Progress_SelectByUserId";
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserProgressModel progress = new UserProgressModel
                            {
                                ProgressId = reader.GetInt32(reader.GetOrdinal("progress_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                Completed = reader.IsDBNull(reader.GetOrdinal("completed")) ? (bool?)null : reader.GetBoolean(reader.GetOrdinal("completed")),
                                CompletionDate = reader.IsDBNull(reader.GetOrdinal("completion_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("completion_date"))
                            };
                            userProgressList.Add(progress);
                        }
                    }
                }
            }
            return userProgressList;
        }

        // Insert a new user progress
        public bool InsertUserProgress(UserProgressModel progress)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Progress_Insert";
                    command.Parameters.AddWithValue("@user_id", progress.UserId);
                    command.Parameters.AddWithValue("@lesson_id", progress.LessonId);
                    command.Parameters.AddWithValue("@completed", progress.Completed);
                    command.Parameters.AddWithValue("@completion_date", (object)progress.CompletionDate ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update user progress
        public bool UpdateUserProgress(UserProgressModel progress)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Progress_Update";
                    command.Parameters.AddWithValue("@progress_id", progress.ProgressId);
                    command.Parameters.AddWithValue("@completed", (object)progress.Completed ?? DBNull.Value);
                    command.Parameters.AddWithValue("@completion_date", (object)progress.CompletionDate ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete user progress
        public bool DeleteUserProgress(int progressId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Progress_Delete";
                    command.Parameters.AddWithValue("@progress_id", progressId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
