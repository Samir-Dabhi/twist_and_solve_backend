namespace twist_and_solve_backend.Models
{
    public class OtpModel
    {
        public string Email { get; set; } = string.Empty;
        public string Otp { get; set; } = string.Empty;
        public DateTime Expiry { get; set; }
    }
}
