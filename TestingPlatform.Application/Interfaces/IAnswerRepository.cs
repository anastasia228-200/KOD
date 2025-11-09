using TestingPlatform.domain.Models;

namespace TestingPlatform.Application.Interfaces;

public interface IAnswerRepository
{
    Task<List<Answer>> GetAllAsync();
    Task<Answer> GetByIdAsync(int id);
    Task<int> CreateAsync(Answer answer);
    Task UpdateAsync(Answer answer);
    Task DeleteAsync(int id);
    Task<List<Answer>> GetByQuestionIdAsync(int questionId); // Дополнительный метод
}