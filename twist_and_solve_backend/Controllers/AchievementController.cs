using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        private readonly AchievementRepository _achievementRepository;

        public AchievementController(AchievementRepository achievementRepository)
        {
            _achievementRepository = achievementRepository;
        }

        // GET: api/Achievement
        [HttpGet]
        public IActionResult GetAllAchievements()
        {
            List<AchievementModel> achievements = _achievementRepository.GetAllAchievements();
            return Ok(achievements);
        }

        // GET: api/Achievement/{id}
        [HttpGet("{id}")]
        public IActionResult GetAchievementById(int id)
        {
            AchievementModel achievement = _achievementRepository.GetAchievementById(id);
            if (achievement == null)
            {
                return NotFound($"Achievement with ID {id} not found.");
            }
            return Ok(achievement);
        }

        // POST: api/Achievement
        [HttpPost]
        public IActionResult AddAchievement([FromBody] AchievementModel achievement)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _achievementRepository.InsertAchievement(achievement);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetAchievementById), new { id = achievement.AchievementId }, achievement);
                }
                return BadRequest("Failed to add the achievement.");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Achievement/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAchievement(int id, [FromBody] AchievementModel achievement)
        {
            if (id != achievement.AchievementId)
            {
                return BadRequest("Achievement ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _achievementRepository.UpdateAchievement(achievement);
                if (isUpdated)
                {
                    return Ok(achievement);
                }
                return BadRequest("Failed to update the achievement.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Achievement/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAchievement(int id)
        {
            bool isDeleted = _achievementRepository.DeleteAchievement(id);
            if (isDeleted)
            {
                return Ok($"Achievement with ID {id} has been deleted.");
            }
            return NotFound($"Achievement with ID {id} not found.");
        }
    }
}
