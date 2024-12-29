namespace twist_and_solve_backend.Models
{
    public class LessonModel
    {
        public int LessonId { get; set; } // Maps to lesson_id

        public string Title { get; set; } // Maps to title

        public string Description { get; set; } // Maps to description

        public int StepOrder { get; set; } // Maps to step_order

        public string ImageUrl { get; set; } // Maps to image_url
    }
}
