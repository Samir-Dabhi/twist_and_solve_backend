using System;

namespace twist_and_solve_backend.Models
{
    public class UserProgressModel
    {
        public int ProgressId { get; set; } // Maps to progress_id
        public int UserId { get; set; } // Maps to user_id
        public int LessonId { get; set; } // Maps to lesson_id
        public bool? Completed { get; set; } // Maps to completed
        public DateTime? CompletionDate { get; set; } // Maps to completion_date
    }
}
