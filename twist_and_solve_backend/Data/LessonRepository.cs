using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Data
{
    public class LessonRepository
    {
        private readonly IConfiguration _configuration;

        public LessonRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all lessons
        public List<LessonModel> GetAllLessons()
        {
            string connectionString = GetConnectionString();
            List<LessonModel> lessons = new List<LessonModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Lessons_SelectAll"; // Assuming you have this stored procedure

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            LessonModel lesson = new LessonModel
                            {
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                StepOrder = reader.GetInt32(reader.GetOrdinal("step_order")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url"))
                            };
                            lessons.Add(lesson);
                        }
                    }
                }
            }
            return lessons;
        }

        // Get a lesson by ID
        public LessonModel GetLessonById(int lessonId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Lessons_SelectById"; // Assuming you have this stored procedure
                    command.Parameters.AddWithValue("@lesson_id", lessonId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new LessonModel
                            {
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                Title = reader.GetString(reader.GetOrdinal("title")),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString(reader.GetOrdinal("description")),
                                StepOrder = reader.GetInt32(reader.GetOrdinal("step_order")),
                                ImageUrl = reader.IsDBNull(reader.GetOrdinal("image_url")) ? null : reader.GetString(reader.GetOrdinal("image_url"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert a lesson
        public bool InsertLesson(LessonModel lesson)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Lessons_Insert"; // Assuming you have this stored procedure
                    command.Parameters.AddWithValue("@title", lesson.Title);
                    command.Parameters.AddWithValue("@description", (object)lesson.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@step_order", lesson.StepOrder);
                    command.Parameters.AddWithValue("@image_url", (object)lesson.ImageUrl ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update a lesson
        public bool UpdateLesson(LessonModel lesson)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Lessons_Update"; // Assuming you have this stored procedure
                    command.Parameters.AddWithValue("@lesson_id", lesson.LessonId);
                    command.Parameters.AddWithValue("@title", (object)lesson.Title ?? DBNull.Value);
                    command.Parameters.AddWithValue("@description", (object)lesson.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@step_order", lesson.StepOrder);
                    command.Parameters.AddWithValue("@image_url", (object)lesson.ImageUrl ?? DBNull.Value);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete a lesson
        public bool DeleteLesson(int lessonId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Lessons_Delete"; // Assuming you have this stored procedure
                    command.Parameters.AddWithValue("@lesson_id", lessonId);

                    sqlConnection.Open();
                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
