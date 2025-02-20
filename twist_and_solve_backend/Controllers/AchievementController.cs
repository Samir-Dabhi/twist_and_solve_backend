using CloudinaryDotNet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;
using System;

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
            try
            {
                List<AchievementModel> achievements = _achievementRepository.GetAllAchievements();
                return Ok(achievements);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        #endregion

        #region Get Achievement by ID
        [HttpGet("{id}")]
        public IActionResult GetAchievementById(int id)
        {
            try
            {
                var achievement = _achievementRepository.GetAchievementById(id);
                if (achievement == null)
                {
                    return NotFound($"Achievement with ID {id} not found.");
                }
                return Ok(achievement);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        #endregion

        #region Add Achievement
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAchievementAsync([FromForm] AchievementUploadModel achievementUpload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var achievement = new AchievementModel
                {
                    AchievementId = achievementUpload.AchievementId,
                    Title = achievementUpload.Title,
                    Description = achievementUpload.Description,
                };

                if (achievementUpload.IconUrl != null)
                {
                    achievement.IconUrl = await _cloudinaryService.UploadImageAsync(achievementUpload.IconUrl);
                }

                bool isInserted = _achievementRepository.InsertAchievement(achievement);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetAchievementById), new { id = achievement.AchievementId }, achievement);
                }
                return BadRequest("Failed to add the achievement.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        #endregion

        #region Update Achievement
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAchievementAsync(int id, [FromForm] AchievementUploadModel achievementUpload)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                if (id != achievementUpload.AchievementId)
                {
                    return BadRequest("Achievement ID mismatch.");
                }

                var achievement = new AchievementModel
                {
                    AchievementId = achievementUpload.AchievementId,
                    Title = achievementUpload.Title,
                    Description = achievementUpload.Description,
                };

                if (achievementUpload.IconUrl != null)
                {
                    achievement.IconUrl = await _cloudinaryService.UploadImageAsync(achievementUpload.IconUrl);
                }

                bool isUpdated = _achievementRepository.UpdateAchievement(achievement);
                if (isUpdated)
                {
                    return Ok(achievement);
                }
                return BadRequest("Failed to update the achievement.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        #endregion

        #region Delete Achievement
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAchievement(int id)
        {
            try
            {
                bool isDeleted = _achievementRepository.DeleteAchievement(id);
                if (isDeleted)
                {
                    return Ok($"Achievement with ID {id} has been deleted.");
                }
                return NotFound($"Achievement with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }
        #endregion
    }
}
