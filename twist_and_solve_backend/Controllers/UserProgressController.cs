using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserProgressController : ControllerBase
    {
        private readonly UserProgressRepository _userProgressRepository;

        public UserProgressController(UserProgressRepository userProgressRepository)
        {
            _userProgressRepository = userProgressRepository;
        }

        // GET: api/UserProgress
        [HttpGet]
        public IActionResult GetAllUserProgress()
        {
            try
            {
                List<UserProgressModel> progressList = _userProgressRepository.GetAllUserProgress();
                if (progressList.Count == 0)
                {
                    return NotFound("No user progress records found.");
                }
                return Ok(progressList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/UserProgress/{userId}
        [HttpGet("{userId}")]
        public IActionResult GetUserProgressByUserId(int userId)
        {
            try
            {
                List<UserProgressModel> progressList = _userProgressRepository.GetUserProgressByUserId(userId);
                if (progressList.Count == 0)
                {
                    return NotFound($"No progress records found for user ID {userId}.");
                }
                return Ok(progressList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/UserProgress
        [HttpPost]
        public IActionResult CreateUserProgress([FromBody] UserProgressModel progress)
        {
            if (progress == null)
            {
                return BadRequest("Invalid progress data.");
            }

            try
            {
                bool isInserted = _userProgressRepository.InsertUserProgress(progress);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetUserProgressByUserId), new { userId = progress.UserId }, progress);
                }
                return BadRequest("Failed to create progress record.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/UserProgress/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateUserProgress(int id, [FromBody] UserProgressModel progress)
        {
            if (progress == null || progress.ProgressId != id)
            {
                return BadRequest("Progress ID mismatch.");
            }

            try
            {
                bool isUpdated = _userProgressRepository.UpdateUserProgress(progress);
                if (isUpdated)
                {
                    return Ok(progress);
                }
                return NotFound($"Progress with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/UserProgress/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteUserProgress(int id)
        {
            try
            {
                bool isDeleted = _userProgressRepository.DeleteUserProgress(id);
                if (isDeleted)
                {
                    return Ok($"Progress record with ID {id} deleted successfully.");
                }
                return NotFound($"Progress record with ID {id} not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
