using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class SolveRepository
    {
        private readonly IConfiguration _configuration;

        public SolveRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all solves
        public List<SolveModel> GetAllSolves()
        {
            string connectionString = GetConnectionString();
            List<SolveModel> solves = new List<SolveModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Solve_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SolveModel solve = new SolveModel
                            {
                                SolveId = reader.GetInt32(reader.GetOrdinal("solve_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                SolveTime = reader.GetDouble(reader.GetOrdinal("solve_time")),
                                SolveDate = reader.GetDateTime(reader.GetOrdinal("solve_date")),
                                Method = reader.GetString(reader.GetOrdinal("method")),
                                MovesCount = reader.IsDBNull(reader.GetOrdinal("moves_count")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("moves_count")),
                                SolveResult = reader.IsDBNull(reader.GetOrdinal("solve_result")) ? null : reader.GetString(reader.GetOrdinal("solve_result")),
                                Scramble = reader.IsDBNull(reader.GetOrdinal("scramble")) ? null : reader.GetString(reader.GetOrdinal("scramble"))
                            };
                            solves.Add(solve);
                        }
                    }
                }
            }
            return solves;
        }

        // Get solves by user ID
        public List<SolveModel> GetSolvesByUserId(int userId)
        {
            string connectionString = GetConnectionString();
            List<SolveModel> solves = new List<SolveModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Solve_SelectByUserId";
                    command.Parameters.AddWithValue("@user_id", userId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            SolveModel solve = new SolveModel
                            {
                                SolveId = reader.GetInt32(reader.GetOrdinal("solve_id")),
                                UserId = reader.GetInt32(reader.GetOrdinal("user_id")),
                                SolveTime = reader.GetDouble(reader.GetOrdinal("solve_time")),
                                SolveDate = reader.GetDateTime(reader.GetOrdinal("solve_date")),
                                Method = reader.GetString(reader.GetOrdinal("method")),
                                MovesCount = reader.IsDBNull(reader.GetOrdinal("moves_count")) ? (int?)null : reader.GetInt32(reader.GetOrdinal("moves_count")),
                                SolveResult = reader.IsDBNull(reader.GetOrdinal("solve_result")) ? null : reader.GetString(reader.GetOrdinal("solve_result")),
                                Scramble = reader.IsDBNull(reader.GetOrdinal("scramble")) ? null : reader.GetString(reader.GetOrdinal("scramble"))
                            };
                            solves.Add(solve);
                        }
                    }
                }
            }
            return solves;
        }

        // Insert a new solve record
        public bool InsertSolve(SolveModel solve)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Solve_Insert";
                    command.Parameters.AddWithValue("@user_id", solve.UserId);
                    command.Parameters.AddWithValue("@solve_time", solve.SolveTime);
                    command.Parameters.AddWithValue("@solve_date", solve.SolveDate);
                    command.Parameters.AddWithValue("@method", solve.Method);
                    command.Parameters.AddWithValue("@moves_count", (object)solve.MovesCount ?? DBNull.Value);
                    command.Parameters.AddWithValue("@solve_result", solve.SolveResult);
                    command.Parameters.AddWithValue("@scramble", solve.Scramble);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update an existing solve record
        public bool UpdateSolve(SolveModel solve)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Solve_Update";
                    command.Parameters.AddWithValue("@solve_id", solve.SolveId);
                    command.Parameters.AddWithValue("@solve_time", (object)solve.SolveTime ?? DBNull.Value);
                    command.Parameters.AddWithValue("@solve_date", (object)solve.SolveDate ?? DBNull.Value);
                    command.Parameters.AddWithValue("@method", (object)solve.Method ?? DBNull.Value);
                    command.Parameters.AddWithValue("@moves_count", (object)solve.MovesCount ?? DBNull.Value);
                    command.Parameters.AddWithValue("@solve_result", (object)solve.SolveResult ?? DBNull.Value);
                    command.Parameters.AddWithValue("@scramble", (object)solve.Scramble ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete a solve record
        public bool DeleteSolve(int solveId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Solve_Delete";
                    command.Parameters.AddWithValue("@solve_id", solveId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
