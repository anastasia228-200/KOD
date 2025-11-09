using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.domain.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Text { get; set; }
        [Required]
        public int Number { get; set; }
        [Required]
        public string Description { get; set; }
        public AnswerType AnswerType { get; set; }
        public bool IsScoring { get; set; } = true;
        public int? MaxScore { get; set; }
        public int TestId { get; set; }
        [Required]
        public Test Test { get; set; }
        public List<UserAttemptsAnswer> UserAttemptsAnswer { get; set; }
        public List<Answer> Answers { get; set; }
    }
}