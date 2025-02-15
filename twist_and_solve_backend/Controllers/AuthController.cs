using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("/admin")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        #region Fields
        private readonly JwtService _jwtService;
        private readonly AuthRepository _authRepository;
        #endregion

        #region Constructor
        public AuthController(JwtService jwtService, AuthRepository authRepository)
        {
            _jwtService = jwtService;
            _authRepository = authRepository;
        }
        #endregion

        #region Authentication
        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            var admin  = _authRepository.adminAuth(request.Username, request.Password);
            if (admin == null)
            {
                return NotFound("Wrong User Name of Password");
            }
            var token = _jwtService.GenerateToken("1", "Admin");
            return Ok(new { Token = token });
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
