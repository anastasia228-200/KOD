using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.domain.Models
{
    public class UserAttemptsAnswer
    {
        public int Id { get; set; }
        public bool IsCorrect { get; set; }
        public int ScoreAwarded { get; set; }
        [Required]
        public int AttemptId { get; set; }
        [Required]
        public int QuestionId { get; set; }

        public Attempt Attempt { get; set; }
        public Question Question { get; set; }
        public List<UserSelectedOption> UserSelectedOptions { get; set; }
        public UserTextAnswer UserTextAnswers { get; set; }
    }
}