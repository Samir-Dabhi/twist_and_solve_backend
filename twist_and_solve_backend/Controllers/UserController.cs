using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        #region Fields
        private readonly UserRepository _userRepository;
        private readonly CloudinaryService _cloudinaryService;
        private readonly JwtService _jwtService;
        #endregion

        #region Constructor
        public UserController(UserRepository userRepository, CloudinaryService cloudinaryService, JwtService jwtService)
        {
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
            _jwtService = jwtService;
        }
        #endregion

        #region GetAllUsers
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }
        #endregion

        #region Add User
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> AddUserAsync([FromForm] UserImageUpload user)
        {
            User user1 = new User
            {
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                DateJoined = user.DateJoined,
                ProgressLevel = user.ProgressLevel
            };

            if (ModelState.IsValid)
            {
                if (user.ProfileImage != null)
                {
                    user1.ProfilePicture = await _cloudinaryService.UploadImageAsync(user.ProfileImage);
                }
                bool isInserted = _userRepository.Insert(user1);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
                }
                return BadRequest("Failed to add the user.");
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region Update User
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromForm] UserImageUpload user)
        {
            User user1 = new User
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                PasswordHash = user.PasswordHash,
                DateJoined = user.DateJoined,
                ProgressLevel = user.ProgressLevel
            };

            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                if (user.ProfileImage != null)
                {
                    user1.ProfilePicture = await _cloudinaryService.UploadImageAsync(user.ProfileImage);
                }
                bool isUpdated = _userRepository.Update(user1);
                if (isUpdated)
                {
                    return Ok(user);
                }
                return BadRequest("Failed to update the user.");
            }
            return BadRequest(ModelState);
        }
        #endregion

        #region Delete User
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteUser(int id)
        {
            bool isDeleted = _userRepository.Delete(id);
            if (isDeleted)
            {
                return Ok($"User with ID {id} has been deleted.");
            }
            return NotFound($"User with ID {id} not found.");
        }
        #endregion

        #region Authentication
        [HttpPost("auth")]
        public IActionResult UserAuth([FromBody] UserLogin users)
        {
            var user = _userRepository.UserAuth(users.Email, users.PasswordHash);
            if (user == null)
            {
                return NotFound("email or password is wrong");
            }
            var token = _jwtService.GenerateToken(users.Email, "User");
            return Ok(new { Token = token,User = user});
        }
        #endregion

        #region reset Password
        [HttpPost("resetpassword")]
        [Authorize(Roles = "Reset")]
        public IActionResult ResetPassword([FromBody] UserLogin data)
        {
            string email = data.Email;
            string newPassword = data.PasswordHash;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(newPassword))
            {
                return BadRequest(new { message = "Email and new password are required" });
            }

            bool isUpdated = _userRepository.UpdatePassword(email, newPassword);
            if (isUpdated)
                return Ok(new { message = "Password reset successfully" });

            return BadRequest(new { message = "Failed to reset password" });
        }
        #endregion
    }

    #region Models
    public class UserLogin
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
    }
    #endregion
}
