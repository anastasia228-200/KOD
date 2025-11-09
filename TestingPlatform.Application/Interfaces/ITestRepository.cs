using TestingPlatform.Application.Dtos;
using TestingPlatform.domain.Models;

namespace TestingPlatform.Application.Interfaces;

public interface ITestRepository
{
    Task<IEnumerable<TestDto>> GetAllAsync(bool? isPublic, List<int> groupIds, List<int> studentsIds);
    Task<IEnumerable<TestDto>> GetAllForStudentAsync(int studentId);
    Task<TestDto> GetByIdAsync(int id);
    Task<int> CreateAsync(TestDto testDto);
    Task UpdateAsync(TestDto testDto);
    Task DeleteAsync(int id);
    Task<IEnumerable<TestDto>> GetTopRecentAsync(int count = 5);
}