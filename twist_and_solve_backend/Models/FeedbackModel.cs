namespace twist_and_solve_backend.Models
{
    public class FeedbackModel
    {
        public int FeedbackId { get; set; } // Maps to feedback_id

        public int UserId { get; set; } // Maps to user_id

        public int? LessonId { get; set; } // Maps to lesson_id (nullable)

        public int Rating { get; set; } // Maps to rating

        public string? Comment { get; set; } // Maps to comment

        public DateTime FeedbackDate { get; set; } // Maps to feedback_date
    }
}
