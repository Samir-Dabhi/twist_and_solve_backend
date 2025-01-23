using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;
using Microsoft.Extensions.Configuration;

namespace twist_and_solve_backend.Data
{
    public class UserAchievementRepository
    {
        private readonly IConfiguration _configuration;

        public UserAchievementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all user achievements
        public List<UserAchievementModel> GetAllUserAchievements()
        {
            string connectionString = GetConnectionString();
            List<UserAchievementModel> userAchievements = new List<UserAchievementModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Achievement_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserAchievementModel userAchievement = new UserAchievementModel
                            {
                                UserAchievementId = reader.GetInt32(reader.GetOrdinal("user_achievement_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                AchievementId = reader.GetInt32(reader.GetOrdinal("achievement_id")),
                                DateEarned = reader.IsDBNull(reader.GetOrdinal("date_earned"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("date_earned"))
                            };
                            userAchievements.Add(userAchievement);
                        }
                    }
                }
            }
            return userAchievements;
        }

        // Get user achievements by user ID
        public List<UserAchievementModel> GetUserAchievementsByUserId(int userId)
        {
            string connectionString = GetConnectionString();
            List<UserAchievementModel> userAchievements = new List<UserAchievementModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Achievement_SelectByUserId";
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            UserAchievementModel userAchievement = new UserAchievementModel
                            {
                                UserAchievementId = reader.GetInt32(reader.GetOrdinal("user_achievement_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                AchievementId = reader.GetInt32(reader.GetOrdinal("achievement_id")),
                                DateEarned = reader.IsDBNull(reader.GetOrdinal("date_earned"))
                                    ? (DateTime?)null
                                    : reader.GetDateTime(reader.GetOrdinal("date_earned")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                            };
                            userAchievements.Add(userAchievement);
                        }
                    }
                }
            }
            return userAchievements;
        }

        // Insert a user achievement
        public bool InsertUserAchievement(UserAchievementModel userAchievement)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Achievement_Insert";
                    command.Parameters.AddWithValue("@user_id", userAchievement.UserId);
                    command.Parameters.AddWithValue("@achievement_id", userAchievement.AchievementId);
                    command.Parameters.AddWithValue("@date_earned", userAchievement.DateEarned ?? (object)DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update a user achievement
        public bool UpdateUserAchievement(UserAchievementModel userAchievement)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Achievement_Update";
                    command.Parameters.AddWithValue("@user_achievement_id", userAchievement.UserAchievementId);
                    command.Parameters.AddWithValue("@user_id", (object)userAchievement.UserId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@achievement_id", (object)userAchievement.AchievementId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@date_earned", (object)userAchievement.DateEarned ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete a user achievement
        public bool DeleteUserAchievement(int userAchievementId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Achievement_Delete";
                    command.Parameters.AddWithValue("@user_achievement_id", userAchievementId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
