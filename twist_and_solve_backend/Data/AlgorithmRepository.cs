using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class AlgorithmRepository
    {
        private readonly IConfiguration _configuration;

        public AlgorithmRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all algorithms
        public List<AlgorithmModel> GetAllAlgorithms()
        {
            string connectionString = GetConnectionString();
            List<AlgorithmModel> algorithms = new List<AlgorithmModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Algorithm_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AlgorithmModel algorithm = new AlgorithmModel
                            {
                                AlgorithmId = reader.GetInt32(reader.GetOrdinal("algorithm_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Notation = reader.GetString(reader.GetOrdinal("notation")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.IsDBNull(reader.GetOrdinal("lesson_id")) ? null : reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                                category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category"))
                            };
                            algorithms.Add(algorithm);
                        }
                    }
                }
            }
            return algorithms;
        }

        // Get algorithm by ID
        public AlgorithmModel GetAlgorithmById(int algorithmId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Algorithm_SelectById";
                    command.Parameters.AddWithValue("@algorithm_id", algorithmId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new AlgorithmModel
                            {
                                AlgorithmId = reader.GetInt32(reader.GetOrdinal("algorithm_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Notation = reader.GetString(reader.GetOrdinal("notation")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.IsDBNull(reader.GetOrdinal("lesson_id")) ? null : reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                                category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Get algorithms by LessonId
        public List<AlgorithmModel> GetAlgorithmsByLessonId(int lessonId)
        {
            string connectionString = GetConnectionString();
            List<AlgorithmModel> algorithms = new List<AlgorithmModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Algorithm_SelectByLessonId";
                    command.Parameters.AddWithValue("@lesson_id", lessonId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AlgorithmModel algorithm = new AlgorithmModel
                            {
                                AlgorithmId = reader.GetInt32(reader.GetOrdinal("algorithm_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Notation = reader.GetString(reader.GetOrdinal("notation")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.IsDBNull(reader.GetOrdinal("lesson_id")) ? null : reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                                category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category"))
                            };
                            algorithms.Add(algorithm);
                        }
                    }
                }
            }
            return algorithms;
        }

        // Get Algorithm by category
        public List<AlgorithmModel> GetAlgorithmsByCategory(String category)
        {
            string connectionString = GetConnectionString();
            List<AlgorithmModel> algorithms = new List<AlgorithmModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Algorithm_Select_By_Category";
                    command.Parameters.AddWithValue("@category", category);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            AlgorithmModel algorithm = new AlgorithmModel
                            {
                                AlgorithmId = reader.GetInt32(reader.GetOrdinal("algorithm_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Notation = reader.GetString(reader.GetOrdinal("notation")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.IsDBNull(reader.GetOrdinal("lesson_id")) ? null : reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url")),
                                category = reader.IsDBNull(reader.GetOrdinal("category")) ? null : reader.GetString(reader.GetOrdinal("category"))
                            };
                            algorithms.Add(algorithm);
                        }
                    }
                }
            }
            return algorithms;
        }

        // Insert algorithm
        public bool Insert(AlgorithmModel algorithm)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_tbl_Algorithm_Insert", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@name", algorithm.Name);
                    cmd.Parameters.AddWithValue("@notation", algorithm.Notation);
                    cmd.Parameters.AddWithValue("@description", (object)algorithm.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lesson_id", (object)algorithm.LessonId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@image_url", (object)algorithm.ImageUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@category", (object)algorithm.category ?? DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"Error Inserting user: {ex.Message}");
                return false;
            }
        }

        // Update algorithm
        public bool Update(AlgorithmModel algorithm)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_tbl_Algorithm_Update", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@algorithm_id", algorithm.AlgorithmId);
                    cmd.Parameters.AddWithValue("@name", algorithm.Name);
                    cmd.Parameters.AddWithValue("@notation", algorithm.Notation);
                    cmd.Parameters.AddWithValue("@description", (object)algorithm.Description ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@lesson_id", (object)algorithm.LessonId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@image_url", (object)algorithm.ImageUrl ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@category", (object)algorithm.category ?? DBNull.Value);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
            catch (Exception ex) 
            {
                    Console.WriteLine($"Error Updating user: {ex.Message}");
                    return false;
            }
        }

        // Delete algorithm
        public bool Delete(int algorithmId)
        {
            string connectionString = GetConnectionString();
            try
            {
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    SqlCommand cmd = new SqlCommand("PR_tbl_Algorithm_Delete", conn)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    cmd.Parameters.AddWithValue("@algorithm_id", algorithmId);

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
    }
}
