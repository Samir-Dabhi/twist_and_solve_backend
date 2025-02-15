using System.Net.Mail;
using System.Net;
using twist_and_solve_backend.Models;
using Microsoft.Data.SqlClient;
using System.Data;

namespace twist_and_solve_backend.Services
{
    public class OtpService
    {
        private static readonly Dictionary<string, OtpModel> _otpStorage = new();
        public string GenerateOtp()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
        }

        public void StoreOtp(string email, string otp)
        {
            _otpStorage[email] = new OtpModel
            {
                Email = email,
                Otp = otp,
                Expiry = DateTime.UtcNow.AddMinutes(5) // OTP valid for 5 minutes
            };
        }

        public bool ValidateOtp(string email, string otp)
        {
            if (_otpStorage.ContainsKey(email) &&
                _otpStorage[email].Otp == otp &&
                _otpStorage[email].Expiry > DateTime.UtcNow)
            {
                _otpStorage.Remove(email);
                return true;
            }
            return false;
        }

        public async Task<bool> SendOtpEmailAsync(string email, string otp)
        {
            try
            {
                using var client = new SmtpClient("smtp.gmail.com") // Change SMTP if needed
                {
                    Port = 587,
                    Credentials = new NetworkCredential("dabhisamir6@gmail.com", "zgdk ckji mubn xxtb"),
                    EnableSsl = true,
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("dabhisamir6@gmail.com"),
                    Subject = "Your OTP Code",
                    Body = $"Your OTP is: {otp}. It is valid for 5 minutes.",
                    IsBodyHtml = false,
                };

                mailMessage.To.Add(email);
                await client.SendMailAsync(mailMessage);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Email sending failed: {ex.Message}");
                return false;
            }
        }
    }
}
