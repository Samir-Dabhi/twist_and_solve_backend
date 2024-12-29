namespace twist_and_solve_backend.Models
{
    public class SettingsModel
    {
        public int SettingId { get; set; } // Maps to setting_id

        public int UserId { get; set; } // Maps to user_id

        public bool DarkMode { get; set; } // Maps to dark_mode

        public bool Notifications { get; set; } // Maps to notifications

        public string Language { get; set; } // Maps to language
    }
}
