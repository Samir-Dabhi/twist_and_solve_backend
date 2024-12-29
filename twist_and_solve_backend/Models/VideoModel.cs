namespace twist_and_solve_backend.Models
{
    public class VideoModel
    {
        public int VideoId { get; set; } // Maps to video_id

        public string Name { get; set; } // Maps to name

        public string Description { get; set; } // Maps to description

        public int LessonId { get; set; } // Maps to lesson_id

        public string VideoUrl { get; set; } // Maps to video_url

        public string ImageUrl { get; set; } // Maps to image_url
    }
}
