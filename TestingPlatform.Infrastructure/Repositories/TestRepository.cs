using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.domain.Models;

namespace TestingPlatform.Infrastructure.Repositories;

public class TestRepository(AppDbContext appDbContext, IMapper mapper) : ITestRepository
{
    public async Task<int> CreateAsync(TestDto testDto)
    {
        var test = mapper.Map<Test>(testDto);

        var testId = await appDbContext.AddAsync(test);

        await UpdateMembersTest(test, testDto);

        await appDbContext.SaveChangesAsync();

        return testId.Entity.Id;
    }

    public async Task DeleteAsync(int id)
    {
        var test = await appDbContext.Tests.FirstOrDefaultAsync(x => x.Id == id);

        if (test == null)
            throw new Exception("Тест не найден");
        appDbContext.Tests.Remove(test);
        await appDbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<TestDto>> GetAllAsync(
        bool? isPublic,
        List<int> groupIds,
        List<int> studentsIds)
    {
        var tests = appDbContext.Tests
            .OrderByDescending(x => x.PublishedAt)
            .ThenBy(t => t.Title)
            .AsNoTracking()
            .AsQueryable();

        if (isPublic is not null)
            tests = tests.Where(t => t.IsPublic == isPublic);

        if (studentsIds.Any())
            tests = tests.Where(t => t.Students.Any(s => studentsIds.Contains(s.Id)));

        if (studentsIds.Any())
            tests = tests.Where(t => t.Groups.Any(g => groupIds.Contains(g.Id)));

        var result = await tests.ToListAsync();

        return mapper.Map<IEnumerable<TestDto>>(result);
    }

    public async Task<IEnumerable<TestDto>> GetAllForStudentAsync(int studentId)
    {
        var tests = await appDbContext.Tests
            .Where(t => t.IsPublic)
            .Where(t =>
                t.Students.Any(s => s.Id == studentId)
                || t.Courses.Any(c => c.Groups.Any(g => g.Students.Any(s => s.Id == studentId)))
                || t.Projects.Any(c => c.Groups.Any(g => g.Students.Any(s => s.Id == studentId)))
                || t.Directions.Any(c => c.Groups.Any(g => g.Students.Any(s => s.Id == studentId))))
            .ToListAsync();
        return mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task<TestDto> GetByIdAsync(int id)
    {
        var test = await appDbContext.Tests
            .Include(t => t.Directions)
            .Include(t => t.Courses)
            .Include(t => t.Projects)
            .Include(t => t.Groups)
            .Include(t => t.Students)
                .ThenInclude(s => s.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);

        if (test == null)
            throw new Exception("Тест не найден");

        return mapper.Map<TestDto>(test);
    }

    public async Task<IEnumerable<TestDto>> GetTopRecentAsync(int count = 5)
    {
        var tests = await appDbContext.Tests.AsNoTracking()
            .OrderByDescending(t => t.PublishedAt)
            .ThenByDescending(t => t.Id)
            .Take(count)
            .ToListAsync();

        return mapper.Map<IEnumerable<TestDto>>(tests);
    }

    public async Task UpdateAsync(TestDto testDto)
    {
        var test = await appDbContext.Tests.FirstOrDefaultAsync(t => t.Id == testDto.Id);

        if (test == null)
            throw new Exception("Тест не найден");

        test.Title = testDto.Title;
        test.Description = testDto.Description;
        test.IsRepeatable = testDto.IsRepeatable;
        test.Type = testDto.Type;
        test.PublishedAt = testDto.PublishedAt;
        test.DeadLine = testDto.DeadLine;
        test.DurationMinutes = testDto.DurationMinutes;
        test.IsPublic = testDto.IsPublic;
        test.PassingScore = testDto.PassingScore;
        test.MaxAttempts = testDto.MaxAttempts;

        await UpdateMembersTest(test, testDto);

        await appDbContext.SaveChangesAsync();
    }

    private async Task UpdateMembersTest(Test test, TestDto testDto)
    {
        var studentIds = testDto.Students?.Select(s => s.Id)
            .Where(id => id > 0)
            .Distinct()
            .ToArray() ?? Array.Empty<int>();

        var groupIds = testDto.Groups?.Select(s => s.Id)
            .Where(id => id > 0)
            .Distinct()
            .ToArray() ?? Array.Empty<int>();

        var courseIds = testDto.Courses?.Select(s => s.Id)
            .Where(id => id > 0)
            .Distinct()
            .ToArray() ?? Array.Empty<int>();

        var directionIds = testDto.Directions?.Select(s => s.Id)
            .Where(id => id > 0)
            .Distinct()
            .ToArray() ?? Array.Empty<int>();

        var projectIds = testDto.Projects?.Select(s => s.Id)
            .Where(id => id > 0)
            .Distinct()
            .ToArray() ?? Array.Empty<int>();

        if (appDbContext.Entry(test).State == EntityState.Detached)
            appDbContext.Attach(test);

        await appDbContext.Entry(test).Collection(t => t.Students).LoadAsync();
        test.Students.Clear();
        if (studentIds.Length > 0)
        {
            var students = await appDbContext.Students
                .Where(s => studentIds.Contains(s.Id))
                .ToListAsync();

            foreach (var s in students)
                test.Students.Add(s);
        }

        await appDbContext.Entry(test).Collection(t => t.Groups).LoadAsync();
        test.Groups.Clear();
        if (groupIds.Length > 0)
        {
            var groups = await appDbContext.Groups
                .Where(g => groupIds.Contains(g.Id))
                .ToListAsync();

            foreach (var g in groups)
                test.Groups.Add(g);
        }

        await appDbContext.Entry(test).Collection(t => t.Courses).LoadAsync();
        test.Courses.Clear();
        if (courseIds.Length > 0)
        {
            var courses = await appDbContext.Courses
                .Where(c => courseIds.Contains(c.Id))
                .ToListAsync();

            foreach (var c in courses)
                test.Courses.Add(c);
        }

        await appDbContext.Entry(test).Collection(t => t.Directions).LoadAsync();
        test.Directions.Clear();
        if (directionIds.Length > 0)
        {
            var directions = await appDbContext.Directions
                .Where(d => directionIds.Contains(d.Id))
                .ToListAsync();

            foreach (var d in directions)
                test.Directions.Add(d);
        }

        await appDbContext.Entry(test).Collection(t => t.Projects).LoadAsync();
        test.Projects.Clear();
        if (projectIds.Length > 0)
        {
            var projects = await appDbContext.Projects
                .Where(p => projectIds.Contains(p.Id))
                .ToListAsync();

            foreach (var p in projects)
                test.Projects.Add(p);
        }
    }

    public async Task RefreshPublicationStatusAsync()
    {
        var now = DateTimeOffset.UtcNow;

        var publishCandidates = await appDbContext.Tests
            .AsNoTracking()
            .Where(t => t.IsPublic && (t.PublishedAt != null || t.DeadLine != null))
            .Select(t => new { t.Id, t.PublishedAt, t.DeadLine })
            .ToListAsync();

        var toPublishIds = publishCandidates
            .Where(x => x.PublishedAt != null
                && x.PublishedAt <= now
                && (x.DeadLine == null || x.DeadLine > now))
            .Select(x => x.Id)
            .ToList();

        if (toPublishIds.Count > 0)
            await appDbContext.Tests
                .Where(t => toPublishIds.Contains(t.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsPublic, true));

        var unpublishCandidates = await appDbContext.Tests
            .AsNoTracking()
            .Where(t => t.IsPublic && (t.PublishedAt == null || t.DeadLine != null))
            .Select(t => new { t.Id, t.PublishedAt, t.DeadLine })
            .ToListAsync();

        var toUnpublishIds = unpublishCandidates
            .Where(x => x.PublishedAt == null || (x.DeadLine != null && x.DeadLine <= now))
            .Select(x => x.Id)
            .ToList();

        if (toUnpublishIds.Count > 0)
            await appDbContext.Tests
                .Where(t => toUnpublishIds.Contains(t.Id))
                .ExecuteUpdateAsync(s => s.SetProperty(t => t.IsPublic, false));
    }

    public async Task<IEnumerable<object>> GetTestByTypeAsync()
    {
        var t = await appDbContext.Tests
            .AsNoTracking()
            .GroupBy(t => t.Type)
            .Select(g => new
            {
                Type = g.Key,
                Count = g.Count()
            })
            .ToListAsync();
        return t;
    }

    public async Task<IEnumerable<object>> GetTestTimelineByPublicAsync()
    {
        return await appDbContext.Tests
            .AsNoTracking()
            .Where(t => t.PublishedAt != default)
            .GroupBy(t => new
            {
                t.IsPublic,
                Year = t.PublishedAt.Year,
                Month = t.PublishedAt.Month,
            })
            .Select(g => new
            {
                g.Key.IsPublic,
                g.Key.Year,
                g.Key.Month,
                Count = g.Count()
            })
            .ToListAsync();
    }
}