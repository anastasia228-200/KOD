namespace TestingPlatform.Requests.Answer;

public class UpdateAnswerRequest
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }
    public int QuestionId { get; set; }
}