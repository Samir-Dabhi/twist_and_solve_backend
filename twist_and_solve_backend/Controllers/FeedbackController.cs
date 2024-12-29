using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackRepository _feedbackRepository;

        public FeedbackController(FeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;
        }

        // GET: api/Feedback
        [HttpGet]
        public IActionResult GetAllFeedbacks()
        {
            List<FeedbackModel> feedbacks = _feedbackRepository.GetAllFeedbacks();
            return Ok(feedbacks);
        }

        // GET: api/Feedback/{id}
        [HttpGet("{id}")]
        public IActionResult GetFeedbackById(int id)
        {
            FeedbackModel feedback = _feedbackRepository.GetFeedbackById(id);
            if (feedback == null)
            {
                return NotFound($"Feedback with ID {id} not found.");
            }
            return Ok(feedback);
        }

        // POST: api/Feedback
        [HttpPost]
        public IActionResult AddFeedback([FromBody] FeedbackModel feedback)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _feedbackRepository.InsertFeedback(feedback);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetFeedbackById), new { id = feedback.FeedbackId }, feedback);
                }
                return BadRequest("Failed to add the feedback.");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Feedback/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateFeedback(int id, [FromBody] FeedbackModel feedback)
        {
            if (id != feedback.FeedbackId)
            {
                return BadRequest("Feedback ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _feedbackRepository.UpdateFeedback(feedback);
                if (isUpdated)
                {
                    return Ok(feedback);
                }
                return BadRequest("Failed to update the feedback.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Feedback/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteFeedback(int id)
        {
            bool isDeleted = _feedbackRepository.DeleteFeedback(id);
            if (isDeleted)
            {
                return Ok($"Feedback with ID {id} has been deleted.");
            }
            return NotFound($"Feedback with ID {id} not found.");
        }
    }
}
