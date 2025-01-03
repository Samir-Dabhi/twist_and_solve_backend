﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using twist_and_solve_backend.Data;
using twist_and_solve_backend.Models;

namespace twist_and_solve_backend.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    public class AlgorithmController : ControllerBase
    {
        private readonly AlgorithmRepository _algorithmRepository;

        public AlgorithmController(AlgorithmRepository algorithmRepository)
        {
            _algorithmRepository = algorithmRepository;
        }

        // GET: /Algorithm
        [HttpGet]
        public IActionResult GetAllAlgorithms()
        {
            var algorithms = _algorithmRepository.GetAllAlgorithms();
            return Ok(algorithms);
        }

        // GET: /Algorithm/{id}
        [HttpGet("{id}")]
        public IActionResult GetAlgorithmById(int id)
        {
            var algorithm = _algorithmRepository.GetAlgorithmById(id);
            if (algorithm == null)
            {
                return NotFound($"Algorithm with ID {id} not found.");
            }
            return Ok(algorithm);
        }

        // GET: /Algorithm/Lesson/{lessonId}
        [HttpGet("Lesson/{lessonId}")]
        public IActionResult GetAlgorithmsByLessonId(int lessonId)
        {
            var algorithms = _algorithmRepository.GetAlgorithmsByLessonId(lessonId);
            if (algorithms == null || algorithms.Count == 0)
            {
                return NotFound($"No algorithms found for Lesson ID {lessonId}.");
            }
            return Ok(algorithms);
        }

        // POST: /Algorithm
        [HttpPost]
        public IActionResult AddAlgorithm([FromBody] AlgorithmModel algorithm)
        {
            if (ModelState.IsValid)
            {
                bool isInserted = _algorithmRepository.Insert(algorithm);
                if (isInserted)
                {
                    return CreatedAtAction(nameof(GetAlgorithmById), new { id = algorithm.AlgorithmId }, algorithm);
                }
                return BadRequest("Failed to add the algorithm.");
            }
            return BadRequest(ModelState);
        }

        // PUT: /Algorithm/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAlgorithm(int id, [FromBody] AlgorithmModel algorithm)
        {
            if (id != algorithm.AlgorithmId)
            {
                return BadRequest("Algorithm ID mismatch.");
            }

            if (ModelState.IsValid)
            {
                bool isUpdated = _algorithmRepository.Update(algorithm);
                if (isUpdated)
                {
                    return Ok(algorithm);
                }
                return BadRequest("Failed to update the algorithm.");
            }
            return BadRequest(ModelState);
        }

        // DELETE: /Algorithm/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAlgorithm(int id)
        {
            bool isDeleted = _algorithmRepository.Delete(id);
            if (isDeleted)
            {
                return Ok($"Algorithm with ID {id} has been deleted.");
            }
            return NotFound($"Algorithm with ID {id} not found.");
        }
    }
}
