namespace twist_and_solve_backend.Models
{
    public class SolveModel
    {
        public int SolveId { get; set; } // Maps to solve_id

        public int UserId { get; set; } // Maps to user_id

        public double SolveTime { get; set; } // Maps to solve_time (Time in seconds)

        public DateTime SolveDate { get; set; } // Maps to solve_date

        public string Method { get; set; } // Maps to method used for solving

        public int? MovesCount { get; set; } // Maps to moves_count

        public string SolveResult { get; set; } // Maps to solve_result (Success or Failed)

        public string Scramble { get; set; } // Maps to scramble
    }
}
