using TestingPlatform.Domain.Enums;

namespace TestingPlatform.Requests.Test;

public class CreateTestRequest
{
    public string Title { get; set; }
    public string Description { get; set; }
    public TestType Type { get; set; }
    public AnswerType AnswerType { get; set; }
    public bool IsRepeatable { get; set; }
    public DateTime PublishedAt { get; set; }
    public DateTime DeadLine { get; set; }
    public int? DurationMinutes { get; set; }
    public bool IsPublic { get; set; }
    public int? PassingScore { get; set; }
    public int? MaxAttempts { get; set; }
}