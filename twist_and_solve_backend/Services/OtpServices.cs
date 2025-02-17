using System.Net.Mail;
using System.Net;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Services
{
    public class OtpService
    {
        private static readonly Dictionary<string, OtpModel> _otpStorage = new();
        private static readonly Dictionary<string, int> _failedAttempts = new();
        private static readonly Dictionary<string, DateTime> _lockoutTimestamps = new();
        private static readonly Dictionary<string, DateTime> _otpRequestTimestamps = new();

        public string GenerateOtp()
        {
            Random random = new();
            return random.Next(100000, 999999).ToString(); // Generates a 6-digit OTP
        }

        public void StoreOtp(string email, string otp)
        {
            if (_otpRequestTimestamps.ContainsKey(email) &&
                (DateTime.UtcNow - _otpRequestTimestamps[email]).TotalSeconds < 60)
            {
                throw new InvalidOperationException("Too many requests. Please wait before requesting another OTP.");
            }

            _otpStorage[email] = new OtpModel
            {
                Email = email,
                Otp = otp,
                Expiry = DateTime.UtcNow.AddMinutes(5)
            };

            _otpRequestTimestamps[email] = DateTime.UtcNow; // Store request time
        }

        public bool ValidateOtp(string email, string otp)
        {
            // Check if the user is locked out
            if (_lockoutTimestamps.ContainsKey(email) && DateTime.UtcNow < _lockoutTimestamps[email])
            {
                return false; // User is locked out
            }

            // Check if OTP is valid
            if (_otpStorage.ContainsKey(email) &&
                _otpStorage[email].Otp == otp &&
                _otpStorage[email].Expiry > DateTime.UtcNow)
            {
                _otpStorage.Remove(email);
                _failedAttempts[email] = 0; // Reset failed attempts on success
                return true;
            }

            // Track failed attempts
            if (!_failedAttempts.ContainsKey(email))
            {
                _failedAttempts[email] = 0;
            }

            _failedAttempts[email]++;

            // Lock user after 5 failed attempts for 10 minutes
            if (_failedAttempts[email] >= 5)
            {
                _lockoutTimestamps[email] = DateTime.UtcNow.AddMinutes(10);
                return false;
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

                string htmlBody = $@"
                <!DOCTYPE html>
                <html>
                <head>
                    <meta charset='UTF-8'>
                    <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                    <title>OTP Verification</title>
                    <style>
                        body {{
                            font-family: Arial, sans-serif;
                            background-color: #f4f4f4;
                            margin: 0;
                            padding: 0;
                        }}
                        .container {{
                            max-width: 500px;
                            margin: 20px auto;
                            background: #ffffff;
                            padding: 20px;
                            border-radius: 8px;
                            box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
                            text-align: center;
                        }}
                        .header {{
                            font-size: 24px;
                            font-weight: bold;
                            color: #112D4E;
                            margin-bottom: 10px;
                        }}
                        .otp {{
                            font-size: 22px;
                            font-weight: bold;
                            color: #FF6B6B;
                            padding: 10px 20px;
                            background: #f8d7da;
                            display: inline-block;
                            border-radius: 5px;
                            margin: 20px 0;
                        }}
                        .message {{
                            font-size: 16px;
                            color: #555;
                        }}
                        .footer {{
                            margin-top: 20px;
                            font-size: 14px;
                            color: #888;
                        }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>Your OTP Code</div>
                        <p class='message'>Use the OTP below to complete your verification:</p>
                        <div class='otp'>{otp}</div>
                        <p class='message'>This OTP is valid for only 5 minutes. Do not share it with anyone.</p>
                        <p class='footer'>If you didn't request this, please ignore this email.</p>
                    </div>
                </body>
                </html>";

                var mailMessage = new MailMessage
                {
                    From = new MailAddress("dabhisamir6@gmail.com"),
                    Subject = "Your OTP Code",
                    Body = htmlBody,
                    IsBodyHtml = true, // Set to true for HTML formatting
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

        public bool IsUserLockedOut(string email)
        {
            return _lockoutTimestamps.ContainsKey(email) && DateTime.UtcNow < _lockoutTimestamps[email];
        }

        public int GetRemainingAttempts(string email)
        {
            if (!_failedAttempts.ContainsKey(email)) return 5; // Default attempts allowed
            return Math.Max(0, 5 - _failedAttempts[email]); // Return remaining attempts
        }

    }
}
