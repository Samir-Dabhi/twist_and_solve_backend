using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;
using twist_and_solve_backend.Services;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        #region Fields
        private readonly AlgorithmRepository _algorithmRepository;
        private readonly CloudinaryService _cloudinaryService;
        #endregion

        #region Constructor
        public AlgorithmController(AlgorithmRepository algorithmRepository, CloudinaryService cloudinaryService)
        {
            _algorithmRepository = algorithmRepository;
            _cloudinaryService = cloudinaryService;
        }
        #endregion

        #region Algorithm Retrieval
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAllAlgorithms()
        {
            var algorithms = _algorithmRepository.GetAllAlgorithms();
            return Ok(algorithms);
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAlgorithmById(int id)
        {
            var algorithm = _algorithmRepository.GetAlgorithmById(id);
            if (algorithm == null)
            {
                return NotFound($"Algorithm with ID {id} not found.");
            }
            return Ok(algorithm);
        }

        [HttpGet("Lesson/{lessonId}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetAlgorithmsByLessonId(int lessonId)
        {
            var algorithms = _algorithmRepository.GetAlgorithmsByLessonId(lessonId);
            if (algorithms == null || algorithms.Count == 0)
            {
                return NotFound($"No algorithms found for Lesson ID {lessonId}.");
            }
            return Ok(algorithms);
        }

        [HttpGet("Category/{category}")]
        [Authorize(Roles = ("Admin,User"))]
        public IActionResult GetAlgorithmsByLess(String category)
        {
            var algorithms = _algorithmRepository.GetAlgorithmsByCategory(category);
            if (algorithms == null || algorithms.Count == 0)
            {
                return NotFound($"No algorithms found this category {category}.");
            }
            return Ok(algorithms);
        }
        #endregion

        #region Algorithm Management
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAlgorithmAsync([FromForm] AlgorithmUploadModel algorithm)
        {
            AlgorithmModel algorithm1 = new AlgorithmModel { 
                Name = algorithm.Name,
                Notation = algorithm.Notation,
                Description = algorithm.Description,
                LessonId = algorithm.LessonId,
                category = algorithm.category,
            };
            if (ModelState.IsValid)
            {
                if (algorithm.ImageUrl!=null)
                {
                    algorithm1.ImageUrl = await _cloudinaryService.UploadImageAsync(algorithm.ImageUrl);
                }
                bool isInserted = _algorithmRepository.Insert(algorithm1);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetAlgorithmById), new { id = algorithm.AlgorithmId }, algorithm);
                }
                return BadRequest("Failed to add the algorithm.");
            }
            return BadRequest(ModelState);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAlgorithmAsync(int id, [FromForm] AlgorithmUploadModel algorithm)
        {
            AlgorithmModel algorithm1 = new AlgorithmModel
            {
                AlgorithmId = algorithm.AlgorithmId,
                Name = algorithm.Name,
                Notation = algorithm.Notation,
                Description = algorithm.Description,
                LessonId = algorithm.LessonId,
                category = algorithm.category,
            };

            if (id != algorithm.AlgorithmId)
            {
                return BadRequest("Algorithm ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                if (algorithm.ImageUrl != null)
                {
                    algorithm1.ImageUrl = await _cloudinaryService.UploadImageAsync(algorithm.ImageUrl);
                }
                bool isUpdated = _algorithmRepository.Update(algorithm1);
                if (isUpdated)
                {
                    return Ok(algorithm);
                }
                return BadRequest("Failed to update the algorithm.");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteAlgorithm(int id)
        {
            bool isDeleted = _algorithmRepository.Delete(id);
            if (isDeleted)
            {
                return Ok($"Algorithm with ID {id} has been deleted.");
            }
            return NotFound($"Algorithm with ID {id} not found.");
        }
        #endregion
    }
}
