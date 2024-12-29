using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserRepository _userRepository;

        public UserController(UserRepository userRepository)
        {
            _userRepository = userRepository;
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
        public IActionResult AddUser([FromBody] User user)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _userRepository.Insert(user);
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
        public IActionResult UpdateUser(int id, [FromBody] User user)
        {
            if (id != user.UserId)
            {
                return BadRequest("User ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _userRepository.Update(user);
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
