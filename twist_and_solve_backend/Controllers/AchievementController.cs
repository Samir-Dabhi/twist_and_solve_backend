using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AchievementController : ControllerBase
    {
        #region Fields
        private readonly AchievementRepository _achievementRepository;
        private readonly CloudinaryService _cloudinaryService;
        #endregion

        #region Constructor
        public AchievementController(AchievementRepository achievementRepository, CloudinaryService cloudinaryService)
        {
            _achievementRepository = achievementRepository;
            _cloudinaryService = cloudinaryService;
        }
        #endregion

        #region Get All Achievements
        [HttpGet]
        public IActionResult GetAllAchievements()
        {
            List<AchievementModel> achievements = _achievementRepository.GetAllAchievements();
            return Ok(achievements);
        }
        #endregion

        #region Get Achievement by ID
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
        #endregion

        #region Add Achievement
        [HttpPost]
        [Authorize(Roles = "Admin")]
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
        #endregion

        #region Update Achievement
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
        #endregion

        #region Delete Achievement
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
        #endregion
    }
}
