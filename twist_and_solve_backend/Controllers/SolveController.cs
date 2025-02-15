using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    #region Controller
    [Route("[controller]")]
    [ApiController]
    public class SolveController : ControllerBase
    {
        #region Fields
        private readonly SolveRepository _solveRepository;
        #endregion

        #region Constructor
        public SolveController(SolveRepository solveRepository)
        {
            _solveRepository = solveRepository;
        }
        #endregion

        #region Solve Retrieval
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllSolves()
        {
            List<SolveModel> solves = _solveRepository.GetAllSolves();
            if (solves == null || solves.Count == 0)
            {
                return NotFound("No solves found.");
            }
            return Ok(solves);
        }

        [HttpGet("user/{userId}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetSolvesByUserId(int userId)
        {
            List<SolveModel> solves = _solveRepository.GetSolvesByUserId(userId);
            if (solves == null || solves.Count == 0)
            {
                return NotFound($"No solves found for user ID {userId}.");
            }
            return Ok(solves);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetSolveById(int id)
        {
            var solve = _solveRepository.GetAllSolves().FirstOrDefault(s => s.SolveId == id);
            if (solve == null)
            {
                return NotFound($"Solve with ID {id} not found.");
            }
            return Ok(solve);
        }
        #endregion

        #region Solve Management
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
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

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,User")]
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

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteSolve(int id)
        {
            bool isDeleted = _solveRepository.DeleteSolve(id);
            if (isDeleted)
            {
                return Ok($"Solve with ID {id} has been deleted.");
            }
            return NotFound($"Solve with ID {id} not found.");
        }
        #endregion
    }
    #endregion
}
