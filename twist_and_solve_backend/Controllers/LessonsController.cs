using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class LessonController : ControllerBase
    {
        private readonly LessonRepository _lessonRepository;

        public LessonController(LessonRepository lessonRepository)
        {
            _lessonRepository = lessonRepository;
        }

        // GET: api/Lesson
        [HttpGet]
        public IActionResult GetAllLessons()
        {
            List<LessonModel> lessons = _lessonRepository.GetAllLessons();
            return Ok(lessons);
        }

        // GET: api/Lesson/{id}
        [HttpGet("{id}")]
        public IActionResult GetLessonById(int id)
        {
            LessonModel lesson = _lessonRepository.GetLessonById(id);
            if (lesson == null)
            {
                return NotFound($"Lesson with ID {id} not found.");
            }
            return Ok(lesson);
        }

        // POST: api/Lesson
        [HttpPost]
        public IActionResult AddLesson([FromBody] LessonModel lesson)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _lessonRepository.InsertLesson(lesson);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetLessonById), new { id = lesson.LessonId }, lesson);
                }
                return BadRequest("Failed to add the lesson.");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Lesson/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateLesson(int id, [FromBody] LessonModel lesson)
        {
            if (id != lesson.LessonId)
            {
                return BadRequest("Lesson ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _lessonRepository.UpdateLesson(lesson);
                if (isUpdated)
                {
                    return Ok(lesson);
                }
                return BadRequest("Failed to update the lesson.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Lesson/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteLesson(int id)
        {
            bool isDeleted = _lessonRepository.DeleteLesson(id);
            if (isDeleted)
            {
                return Ok($"Lesson with ID {id} has been deleted.");
            }
            return NotFound($"Lesson with ID {id} not found.");
        }
    }
}
