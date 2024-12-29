using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SolveController : ControllerBase
    {
        private readonly SolveRepository _solveRepository;

        public SolveController(SolveRepository solveRepository)
        {
            _solveRepository = solveRepository;
        }

        // GET: api/Solve
        [HttpGet]
        public IActionResult GetAllSolves()
        {
            List<SolveModel> solves = _solveRepository.GetAllSolves();
            if (solves == null || solves.Count == 0)
            {
                return NotFound("No solves found.");
            }
            return Ok(solves);
        }

        // GET: api/Solve/{userId}
        [HttpGet("user/{userId}")]
        public IActionResult GetSolvesByUserId(int userId)
        {
            List<SolveModel> solves = _solveRepository.GetSolvesByUserId(userId);
            if (solves == null || solves.Count == 0)
            {
                return NotFound($"No solves found for user ID {userId}.");
            }
            return Ok(solves);
        }

        // GET: api/Solve/{id}
        [HttpGet("{id}")]
        public IActionResult GetSolveById(int id)
        {
            var solve = _solveRepository.GetAllSolves().FirstOrDefault(s => s.SolveId == id);
            if (solve == null)
            {
                return NotFound($"Solve with ID {id} not found.");
            }
            return Ok(solve);
        }

        // POST: api/Solve
        [HttpPost]
        public IActionResult AddSolve([FromBody] SolveModel solve)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _solveRepository.InsertSolve(solve);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetSolveById), new { id = solve.SolveId }, solve);
                }
                return BadRequest("Failed to add the solve.");
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Solve/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateSolve(int id, [FromBody] SolveModel solve)
        {
            if (id != solve.SolveId)
            {
                return BadRequest("Solve ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _solveRepository.UpdateSolve(solve);
                if (isUpdated)
                {
                    return Ok(solve);
                }
                return BadRequest("Failed to update the solve.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: api/Solve/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteSolve(int id)
        {
            bool isDeleted = _solveRepository.DeleteSolve(id);
            if (isDeleted)
            {
                return Ok($"Solve with ID {id} has been deleted.");
            }
            return NotFound($"Solve with ID {id} not found.");
        }
    }
}
