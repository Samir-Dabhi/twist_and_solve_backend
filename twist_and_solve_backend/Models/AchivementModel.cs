namespace twist_and_solve_backend.Models
{
    public class AchievementModel
    {
        public int AchievementId { get; set; } // Maps to achievement_id

        public string Title { get; set; } // Maps to title

        public string Description { get; set; } // Maps to description

        public string IconUrl { get; set; } // Maps to icon_url
    }
}
