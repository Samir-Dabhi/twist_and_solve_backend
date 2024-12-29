using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class SettingsController : ControllerBase
    {
        private readonly SettingsRepository _settingsRepository;

        public SettingsController(SettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        // GET: api/Settings
        [HttpGet]
        public IActionResult GetAllSettings()
        {
            List<SettingsModel> settings = _settingsRepository.GetAllSettings();
            return Ok(settings);
        }

        // GET: api/Settings/{id}
        [HttpGet("{id}")]
        public IActionResult GetSettingsById(int id)
        {
            SettingsModel settings = _settingsRepository.GetSettingsById(id);
            if (settings == null)
            {
                return NotFound($"Settings with ID {id} not found.");
            }
            return Ok(settings);
        }

        // GET: api/Settings/user/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetSettingsByUserId(int userId)
        {
            SettingsModel settings = _settingsRepository.GetSettingsByUserId(userId);
            if (settings == null)
            {
                return NotFound($"Settings for User ID {userId} not found.");
            }
            return Ok(settings);
        }

        // POST: api/Settings
        [HttpPost]
        public IActionResult AddSettings([FromBody] SettingsModel settings)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _settingsRepository.InsertSetting(settings);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetSettingsById), new { id = settings.SettingId }, settings);
                }
                return BadRequest("Failed to add the settings.");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Settings/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSettings(int id, [FromBody] SettingsModel settings)
        {
            if (id != settings.SettingId)
            {
                return BadRequest("Settings ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _settingsRepository.UpdateSetting(settings);
                if (isUpdated)
                {
                    return Ok(settings);
                }
                return BadRequest("Failed to update the settings.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Settings/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSettings(int id)
        {
            bool isDeleted = _settingsRepository.DeleteSetting(id);
            if (isDeleted)
            {
                return Ok($"Settings with ID {id} has been deleted.");
            }
            return NotFound($"Settings with ID {id} not found.");
        }
    }
}
