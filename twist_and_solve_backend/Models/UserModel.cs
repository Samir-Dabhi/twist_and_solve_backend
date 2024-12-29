namespace twist_and_solve_backend.Models
{

    public class User
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateJoined { get; set; }
        public string ProfilePicture { get; set; }
        public int ProgressLevel { get; set; }
    }
}
