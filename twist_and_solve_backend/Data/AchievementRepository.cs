using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class AchievementRepository
    {
        private readonly IConfiguration _configuration;

        public AchievementRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all achievements
        public List<AchievementModel> GetAllAchievements()
        {
            string connectionString = GetConnectionString();
            List<AchievementModel> achievements = new List<AchievementModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Achievement_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AchievementModel achievement = new AchievementModel
                            {
                                AchievementId = reader.GetInt32(reader.GetOrdinal("achievement_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                IconUrl = reader.IsDBNull(reader.GetOrdinal("icon_url")) ? null : reader.GetString(reader.GetOrdinal("icon_url"))
                            };
                            achievements.Add(achievement);
                        }
                    }
                }
            }
            return achievements;
        }

        // Get an achievement by ID
        public AchievementModel GetAchievementById(int achievementId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Achievement_SelectById";
                    command.Parameters.AddWithValue("@achievement_id", achievementId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AchievementModel
                            {
                                AchievementId = reader.GetInt32(reader.GetOrdinal("achievement_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                IconUrl = reader.IsDBNull(reader.GetOrdinal("icon_url")) ? null : reader.GetString(reader.GetOrdinal("icon_url"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert an achievement
        public bool InsertAchievement(AchievementModel achievement)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Achievement_Insert";
                    command.Parameters.AddWithValue("@title", achievement.Title);
                    command.Parameters.AddWithValue("@description", (object)achievement.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@icon_url", (object)achievement.IconUrl ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update an achievement
        public bool UpdateAchievement(AchievementModel achievement)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Achievement_Update";
                    command.Parameters.AddWithValue("@achievement_id", achievement.AchievementId);
                    command.Parameters.AddWithValue("@title", (object)achievement.Title ?? DBNull.Value);
                    command.Parameters.AddWithValue("@description", (object)achievement.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@icon_url", (object)achievement.IconUrl ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete an achievement
        public bool DeleteAchievement(int achievementId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Achievement_Delete";
                    command.Parameters.AddWithValue("@achievement_id", achievementId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
