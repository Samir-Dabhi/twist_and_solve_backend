using Microsoft.Data.SqlClient;
using System.Data;
using twist_and_solve_backend.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace twist_and_solve_backend.Data
{
    public class VideoRepository
    {
        private readonly IConfiguration _configuration;

        public VideoRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private string GetConnectionString()
        {
            return _configuration.GetConnectionString("ConnectionString");
        }

        // Get all videos
        public List<VideoModel> GetAllVideos()
        {
            string connectionString = GetConnectionString();
            List<VideoModel> videos = new List<VideoModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Videos_SelectAll";

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VideoModel video = new VideoModel
                            {
                                VideoId = reader.GetInt32(reader.GetOrdinal("video_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                VideoUrl = reader.GetString(reader.GetOrdinal("video_url")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
                            };
                            videos.Add(video);
                        }
                    }
                }
            }
            return videos;
        }

        // Get videos by lesson ID
        public List<VideoModel> GetVideosByLessonId(int lessonId)
        {
            string connectionString = GetConnectionString();
            List<VideoModel> videos = new List<VideoModel>();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Videos_SelectByLessonId";
                    command.Parameters.AddWithValue("@lesson_id", lessonId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            VideoModel video = new VideoModel
                            {
                                VideoId = reader.GetInt32(reader.GetOrdinal("video_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                VideoUrl = reader.GetString(reader.GetOrdinal("video_url")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
                            };
                            videos.Add(video);
                        }
                    }
                }
            }
            return videos;
        }

        // Get video by ID
        public VideoModel GetVideoById(int videoId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Videos_SelectById";
                    command.Parameters.AddWithValue("@video_id", videoId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new VideoModel
                            {
                                VideoId = reader.GetInt32(reader.GetOrdinal("video_id")),
                                Name = reader.GetString(reader.GetOrdinal("name")),
                                Description = reader.GetString(reader.GetOrdinal("description")),
                                LessonId = reader.GetInt32(reader.GetOrdinal("lesson_id")),
                                VideoUrl = reader.GetString(reader.GetOrdinal("video_url")),
                                ImageUrl = reader.GetString(reader.GetOrdinal("image_url"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        // Insert a video
        public bool InsertVideo(VideoModel video)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Videos_Insert";
                    command.Parameters.AddWithValue("@name", video.Name);
                    command.Parameters.AddWithValue("@description", video.Description);
                    command.Parameters.AddWithValue("@lesson_id", video.LessonId);
                    command.Parameters.AddWithValue("@video_url", video.VideoUrl);
                    command.Parameters.AddWithValue("@image_url", video.ImageUrl);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Update a video
        public bool UpdateVideo(VideoModel video)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Videos_Update";
                    command.Parameters.AddWithValue("@video_id", video.VideoId);
                    command.Parameters.AddWithValue("@name", (object)video.Name ?? DBNull.Value);
                    command.Parameters.AddWithValue("@description", (object)video.Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@lesson_id", (object)video.LessonId ?? DBNull.Value);
                    command.Parameters.AddWithValue("@video_url", (object)video.VideoUrl ?? DBNull.Value);
                    command.Parameters.AddWithValue("@image_url", (object)video.ImageUrl ?? DBNull.Value);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }

        // Delete a video
        public bool DeleteVideo(int videoId)
        {
            string connectionString = GetConnectionString();

            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (SqlCommand command = sqlConnection.CreateCommand())
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.CommandText = "PR_tbl_Videos_Delete";
                    command.Parameters.AddWithValue("@video_id", videoId);

                    int rowsAffected = command.ExecuteNonQuery();
                    return rowsAffected > 0;
                }
            }
        }
    }
}
