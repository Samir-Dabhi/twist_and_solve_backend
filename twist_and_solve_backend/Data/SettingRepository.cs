using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class SettingsRepository
    {
        private readonly IConfiguration _configuration;

        public SettingsRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all settings
        public List<SettingsModel> GetAllSettings()
        {
            string connectionString = GetConnectionString();
            List<SettingsModel> settingsList = new List<SettingsModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Setting_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SettingsModel settings = new SettingsModel
                            {
                                SettingId = reader.GetInt32(reader.GetOrdinal("setting_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DarkMode = reader.GetBoolean(reader.GetOrdinal("dark_mode")),
                                Notifications = reader.GetBoolean(reader.GetOrdinal("notifications")),
                                Language = reader.GetString(reader.GetOrdinal("language"))
                            };
                            settingsList.Add(settings);
                        }
                    }
                }
            }
            return settingsList;
        }

        // Get settings by setting ID
        public SettingsModel GetSettingsById(int settingId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Setting_SelectById";
                    command.Parameters.AddWithValue("@setting_id", settingId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SettingsModel
                            {
                                SettingId = reader.GetInt32(reader.GetOrdinal("setting_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DarkMode = reader.GetBoolean(reader.GetOrdinal("dark_mode")),
                                Notifications = reader.GetBoolean(reader.GetOrdinal("notifications")),
                                Language = reader.GetString(reader.GetOrdinal("language"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Get settings by user ID
        public SettingsModel GetSettingsByUserId(int userId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Setting_SelectByUserId";
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new SettingsModel
                            {
                                SettingId = reader.GetInt32(reader.GetOrdinal("setting_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                DarkMode = reader.GetBoolean(reader.GetOrdinal("dark_mode")),
                                Notifications = reader.GetBoolean(reader.GetOrdinal("notifications")),
                                Language = reader.GetString(reader.GetOrdinal("language"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert a new setting
        public bool InsertSetting(SettingsModel settings)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Setting_Insert";
                    command.Parameters.AddWithValue("@user_id", settings.UserId);
                    command.Parameters.AddWithValue("@dark_mode", settings.DarkMode);
                    command.Parameters.AddWithValue("@notifications", settings.Notifications);
                    command.Parameters.AddWithValue("@language", settings.Language);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update settings
        public bool UpdateSetting(SettingsModel settings)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Setting_Update";
                    command.Parameters.AddWithValue("@setting_id", settings.SettingId);
                    command.Parameters.AddWithValue("@dark_mode", settings.DarkMode);
                    command.Parameters.AddWithValue("@notifications", settings.Notifications);
                    command.Parameters.AddWithValue("@language", settings.Language);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete a setting by ID
        public bool DeleteSetting(int settingId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Setting_Delete";
                    command.Parameters.AddWithValue("@setting_id", settingId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
