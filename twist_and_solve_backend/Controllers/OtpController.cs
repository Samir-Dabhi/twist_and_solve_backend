using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        #endregion

        #region Constructor
        public OtpController(JwtService jwtService)
        {
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
                return BadRequest(new { message = "Email is required" });
            }

            string otp = _otpService.GenerateOtp();
            _otpService.StoreOtp(email, otp);
            bool emailSent = await _otpService.SendOtpEmailAsync(email, otp);

            if (emailSent)
                return Ok(new { message = "OTP sent successfully" });

            return StatusCode(500, new { message = "Failed to send OTP" });
        }
        #endregion

        #region OTP Verification
        [HttpPost("verifyotp")]
        public IActionResult VerifyOtp([FromBody] VerifyOtpModel data)
        {
            string email = data.Email;
            string otp = data.Otp;

            if (_otpService.ValidateOtp(email, otp))
            {
                var token = _jwtService.GenerateToken("2", "Reset");
                return Ok(new { message = "OTP verified successfully",Token = token });
            }

            return BadRequest(new { message = "Invalid or expired OTP" });
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
