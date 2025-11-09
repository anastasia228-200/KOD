using System.ComponentModel.DataAnnotations;


namespace TestingPlatform.domain.Models
{
    public class Attempt
    {
        public int Id { get; set; }
        public DateTime StartedAt { get; set; } = DateTime.Now;
        public DateTime? SubmittedAt { get; set; }
        public int? Score { get; set; }
        [Required]
        public int TestId { get; set; }
        [Required]
        public int StudentId { get; set; }

        public Test Test { get; set; }
        public Student Student { get; set; }
        public List<UserAttemptsAnswer> UserAttemptAnswers { get; set; }
    }
}
