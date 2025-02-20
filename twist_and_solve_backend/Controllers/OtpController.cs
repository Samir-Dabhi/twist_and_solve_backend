using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("")]
    [ApiController]
    public class OtpController : ControllerBase
    {
        #region Fields
        private readonly OtpService _otpService;
        private readonly JwtService _jwtService;
        private readonly UserRepository _userRepository;
        #endregion

        #region Constructor
        public OtpController(JwtService jwtService, UserRepository userRepository)
        {
            _userRepository = userRepository;
            _otpService = new OtpService();
            _jwtService = jwtService;
        }
        #endregion

        #region OTP Generation & Sending
        [HttpPost("sendotp")]
        public async Task<IActionResult> SendOtp([FromBody] OtpModel otpreq)
        {
            string email = otpreq.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            User user = _userRepository.GetUserByEmail(email);
            if (user == null)
            {
                return BadRequest(new { message = "No user exists with this email." });
            }

            try
            {
                string otp = _otpService.GenerateOtp();
                _otpService.StoreOtp(email, otp);
                bool emailSent = await _otpService.SendOtpEmailAsync(email, otp);

                if (emailSent)
                    return Ok(new { message = "OTP sent successfully." });

                return StatusCode(500, new { message = "Failed to send OTP. Please try again later." });
            }
            catch (InvalidOperationException ex) // Handle rate limiting
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        #endregion
        [HttpPost("emailauth")]
        public async Task<IActionResult> SendOtpForEmailVerification([FromBody] OtpModel otpreq)
        {
            string email = otpreq.Email;

            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email is required." });
            }
            try
            {
                string otp = _otpService.GenerateOtp();
                _otpService.StoreOtp(email, otp);
                bool emailSent = await _otpService.SendOtpEmailAsync(email, otp);

                if (emailSent)
                    return Ok(new { message = "OTP sent successfully." });

                return StatusCode(500, new { message = "Failed to send OTP. Please try again later." });
            }
            catch (InvalidOperationException ex) // Handle rate limiting
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = $"An error occurred: {ex.Message}" });
            }
        }
        #region OTP Verification
        [HttpPost("verifyotp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpModel data)
        {
            string email = data.Email;
            string otp = data.Otp;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
            {
                return BadRequest(new { message = "Email and OTP are required." });
            }

            if (_otpService.IsUserLockedOut(email))
            {
                return BadRequest(new { message = "Too many failed attempts. Please try again later." });
            }

            if (_otpService.ValidateOtp(email, otp))
            {
                var token = _jwtService.GenerateToken(email + "reset", "Reset");
                return Ok(new { message = "OTP verified successfully.", Token = token });
            }

            int remainingAttempts = _otpService.GetRemainingAttempts(email);
            if (remainingAttempts > 0)
            {
                return BadRequest(new { message = $"Invalid OTP. You have {remainingAttempts} attempts remaining." });
            }

            return BadRequest(new { message = "Too many failed attempts. You are locked out for 10 minutes." });
        }
        #endregion
        #region OTP Verification
        [HttpPost("verifyemailotp")]
        public IActionResult VerifyEmailOtp([FromBody] VerifyOtpModel data)
        {
            string email = data.Email;
            string otp = data.Otp;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(otp))
            {
                return BadRequest(new { message = "Email and OTP are required." });
            }

            if (_otpService.IsUserLockedOut(email))
            {
                return BadRequest(new { message = "Too many failed attempts. Please try again later." });
            }

            if (_otpService.ValidateOtp(email, otp))
            {
                var token = _jwtService.GenerateToken(email + "reset", "Signup");
                return Ok(new { message = "OTP verified successfully.", Token = token });
            }

            int remainingAttempts = _otpService.GetRemainingAttempts(email);
            if (remainingAttempts > 0)
            {
                return BadRequest(new { message = $"Invalid OTP. You have {remainingAttempts} attempts remaining." });
            }

            return BadRequest(new { message = "Too many failed attempts. You are locked out for 10 minutes." });
        }
        #endregion
    }

    #region Models
    public class OtpModel
    {
        public string Email { get; set; }
    }

    public class VerifyOtpModel
    {
        public string Email { get; set; }
        public string Otp { get; set; }
    }

    public class ResetPasswordModel
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
    #endregion
}
