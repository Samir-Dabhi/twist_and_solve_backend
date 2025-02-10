namespace twist_and_solve_backend.Models
{
    public class UserAchievementModel
    {
        public int UserAchievementId { get; set; } // Maps to user_achievement_id
        public int UserId { get; set; } // Maps to user_id
        public int AchievementId { get; set; } // Maps to achievement_id
        public string Title { get; set; } // Maps title of achivement
        public string Description { get; set; } // Maps title of achivement
        public DateTime? DateEarned { get; set; } // Maps to date_earned
        public string IconURL { get; set; } //Maps to icon_url
    }
    public class UserAchievementUploadModel
    {
        public int UserAchievementId { get; set; } // Maps to user_achievement_id
        public int UserId { get; set; } // Maps to user_id
        public int AchievementId { get; set; } // Maps to achievement_id
        public DateTime? DateEarned { get; set; } // Maps to date_earned
    }
}
