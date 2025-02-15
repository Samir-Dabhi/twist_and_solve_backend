using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class UserRepository
    {
        private readonly IConfiguration _configuration;

        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString"); // Your connection string
        }

        // Get all users
        public List<User> GetAllUsers()
        {
            string connectionString = GetConnectionString();
            List<User> users = new List<User>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            User user = new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                                DateJoined = reader.GetDateTime(reader.GetOrdinal("date_joined")),
                                ProfilePicture = reader.GetString(reader.GetOrdinal("profile_picture")),
                                ProgressLevel = reader.GetInt32(reader.GetOrdinal("progress_level"))
                            };
                            users.Add(user);
                        }
                    }
                }
            }
            return users;
        }

        // Get a user by ID
        public User GetUserById(int userId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_SelectById"; // Stored procedure to get user by ID
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                PasswordHash = reader.GetString(reader.GetOrdinal("password_hash")),
                                DateJoined = reader.GetDateTime(reader.GetOrdinal("date_joined")),
                                ProfilePicture = reader.GetString(reader.GetOrdinal("profile_picture")),
                                ProgressLevel = reader.GetInt32(reader.GetOrdinal("progress_level"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert a user
        public bool Insert(User user)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_tbl_User_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@profile_picture", user.ProfilePicture);
                    cmd.Parameters.AddWithValue("@progress_level", user.ProgressLevel);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error inserting user: {ex.Message}");
                return false;
            }
        }

        // Update a user
        public bool Update(User user)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_tbl_User_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@user_id", user.UserId);
                    cmd.Parameters.AddWithValue("@username", user.Username);
                    cmd.Parameters.AddWithValue("@email", user.Email);
                    cmd.Parameters.AddWithValue("@password_hash", user.PasswordHash);
                    cmd.Parameters.AddWithValue("@profile_picture", user.ProfilePicture);
                    cmd.Parameters.AddWithValue("@progress_level", user.ProgressLevel);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

        // Delete a user
        public bool Delete(int userId)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_tbl_User_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@user_id", userId);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting user: {ex.Message}");
                return false;
            }
        }

        public User UserAuth(String email,String password)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_User_Authenticate"; // Stored procedure to get user by ID
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@password_hash", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new User
                            {
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                Username = reader.GetString(reader.GetOrdinal("username")),
                                Email = reader.GetString(reader.GetOrdinal("email")),
                                ProfilePicture = reader.GetString(reader.GetOrdinal("profile_picture")),
                                ProgressLevel = reader.GetInt32(reader.GetOrdinal("progress_level"))
                            };
                        }
                    }
                }
            }
            return null;
        }
        public bool UpdatePassword(string email, string newPassword)
        {

            string connectionString = GetConnectionString();
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
            {
                return false;
            }
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_User_UpdatePassword", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@email", email);
                    cmd.Parameters.AddWithValue("@newPassward", newPassword);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating user: {ex.Message}");
                return false;
            }
        }

    }
}
