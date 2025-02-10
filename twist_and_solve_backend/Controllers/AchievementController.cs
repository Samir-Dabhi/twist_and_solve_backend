using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.Intrinsics.X86;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    
    public class AchievementController : ControllerBase
    {
        private readonly AchievementRepository _achievementRepository;
        private readonly CloudinaryService _cloudinaryService;

        public AchievementController(AchievementRepository achievementRepository, CloudinaryService cloudinaryService)
        {
            _achievementRepository = achievementRepository;
            _cloudinaryService = cloudinaryService; 
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
        public async Task<IActionResult> AddAchievementAsync([FromForm] AchievementUploadModel AchiachievementUpload)
        {
            AchievementModel achievement = new AchievementModel
            {
                AchievementId = AchiachievementUpload.AchievementId,
                Title = AchiachievementUpload.Title,
                Description = AchiachievementUpload.Description,
            };

            if (ModelState.IsValid)
            {
                if (AchiachievementUpload.IconUrl != null)
                {
                    achievement.IconUrl = await _cloudinaryService.UploadImageAsync(AchiachievementUpload.IconUrl);
                }
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
        public async Task<IActionResult> UpdateAchievementAsync(int id, [FromForm] AchievementUploadModel AchiachievementUpload)
        {
            AchievementModel achievement = new AchievementModel
            {
                AchievementId = AchiachievementUpload.AchievementId,
                Title = AchiachievementUpload.Title,
                Description = AchiachievementUpload.Description,
            };
            if (id != achievement.AchievementId)
            {
                return BadRequest("Achievement ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                if (AchiachievementUpload.IconUrl != null)
                {
                    achievement.IconUrl = await _cloudinaryService.UploadImageAsync(AchiachievementUpload.IconUrl);
                }
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