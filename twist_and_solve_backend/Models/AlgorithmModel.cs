namespace twist_and_solve_backend.Models
{
    public class AlgorithmModel
    {
        public int AlgorithmId { get; set; } // Primary key
        public string Name { get; set; }     // Algorithm name
        public string Notation { get; set; } // Algorithm notation (e.g., Rubik's Cube moves)
        public string? Description { get; set; } // Optional description
        public int? LessonId { get; set; }   // Foreign key linking to Lessons table
    }
}
