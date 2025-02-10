using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserAchievementController : ControllerBase
    {
        private readonly UserAchievementRepository _userAchievementRepository;

        public UserAchievementController(UserAchievementRepository userAchievementRepository)
        {
            _userAchievementRepository = userAchievementRepository;
        }

        // GET: api/UserAchievement
        [HttpGet]
        public IActionResult GetAllUserAchievements()
        {
            var userAchievements = _userAchievementRepository.GetAllUserAchievements();
            return Ok(userAchievements);
        }

        // GET: api/UserAchievement/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetUserAchievementsByUserId(int userId)
        {
            var userAchievements = _userAchievementRepository.GetUserAchievementsByUserId(userId);
            if (userAchievements == null || !userAchievements.Any())
            {
                return NotFound($"No achievements found for user with ID {userId}.");
            }
            return Ok(userAchievements);
        }

        // POST: api/UserAchievement
        [HttpPost]
        public IActionResult AddUserAchievement([FromBody] UserAchievementUploadModel userAchievement)
        {
            if (ModelState.IsValid)
            {
                var isInserted = _userAchievementRepository.InsertUserAchievement(userAchievement);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetUserAchievementsByUserId), new { userId = userAchievement.UserId }, userAchievement);
                }
                return BadRequest("Failed to add the user achievement.");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/UserAchievement/{userAchievementId}
        [HttpPut("{userAchievementId}")]
        public IActionResult UpdateUserAchievement(int userAchievementId, [FromBody] UserAchievementUploadModel userAchievement)
        {
            if (userAchievementId != userAchievement.UserAchievementId)
            {
                return BadRequest("User achievement ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                var isUpdated = _userAchievementRepository.UpdateUserAchievement(userAchievement);
                if (isUpdated)
                {
                    return Ok(userAchievement);
                }
                return BadRequest("Failed to update the user achievement.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/UserAchievement/{userAchievementId}
        [HttpDelete("{userAchievementId}")]
        public IActionResult DeleteUserAchievement(int userAchievementId)
        {
            var isDeleted = _userAchievementRepository.DeleteUserAchievement(userAchievementId);
            if (isDeleted)
            {
                return Ok($"User achievement with ID {userAchievementId} has been deleted.");
            }
            return NotFound($"User achievement with ID {userAchievementId} not found.");
        }
    }
}
