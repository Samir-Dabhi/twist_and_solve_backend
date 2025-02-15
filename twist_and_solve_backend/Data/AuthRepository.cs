using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class AuthRepository
    {
        private readonly IConfiguration _configuration;
        public AuthRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }
        public AdminModel adminAuth(String email, String password)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Admin_Login";
                    command.Parameters.AddWithValue("@username", email);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AdminModel
                            {
                                UserName = reader.GetString(reader.GetOrdinal("user_name")),
                            };
                        }
                    }
                }
            }
            return null;
        }
    }
}
