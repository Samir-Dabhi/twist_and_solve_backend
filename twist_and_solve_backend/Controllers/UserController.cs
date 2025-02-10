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
        private readonly UserRepository _userRepository;
        private readonly CloudinaryService _cloudinaryService;

        public UserController(UserRepository userRepository, CloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _cloudinaryService = cloudinaryService;
        }

        // GET: api/User
        [HttpGet]
        public IActionResult GetAllUsers()
        {
            var users = _userRepository.GetAllUsers();
            return Ok(users);
        }

        // GET: api/User/{id}
        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return NotFound($"User with ID {id} not found.");
            }
            return Ok(user);
        }

        // POST: api/User
        [HttpPost]
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

        // PUT: api/User/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, [FromForm] UserImageUpload user)
        {
            User user1 = new User
            {
                UserId=user.UserId,
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

        // DELETE: api/User/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            bool isDeleted = _userRepository.Delete(id);
            if (isDeleted)
            {
                return Ok($"User with ID {id} has been deleted.");
            }
            return NotFound($"User with ID {id} not found.");
        }

        // GET: api/User/auth
        [HttpPost("auth")]
        public IActionResult UserAuth([FromBody] User users)
        {
            var user = _userRepository.UserAuth(users.Email, users.PasswordHash);
            if (user == null)
            {
                return NotFound($"email or password is wrong");
            }
            return Ok(user);
        }

    }
}