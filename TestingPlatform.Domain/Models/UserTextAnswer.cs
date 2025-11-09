using System.ComponentModel.DataAnnotations;

namespace TestingPlatform.domain.Models
{
    public class UserTextAnswer
    {
        public int Id { get; set; }
        [Required]
        public string TextAnswer { get; set; }
        [Required]
        public int UserAttemptAnswerId { get; set; }
        public UserAttemptsAnswer UserAttemptAnswer { get; set; }
    }
}