using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("/admin")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly JwtService _jwtService;
        #endregion

        #region Constructor
        public AuthController(JwtService jwtService)
        {
            _jwtService = jwtService;
        }
        #endregion

        #region Authentication
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (request.Username == "ScorpionKing" && request.Password == "!#(!#^")
            {
                var token = _jwtService.GenerateToken("1", "Admin");
                return Ok(new { Token = token });
            }

            return Unauthorized();
        }
        #endregion
    }
    #region Models
    public class LoginRequest
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
    #endregion
}
