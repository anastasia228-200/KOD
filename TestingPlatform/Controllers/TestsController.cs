using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

namespace TestingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        private readonly ITestRepository _testRepository;

        public TestsController(ITestRepository testRepository)
        {
            _testRepository = testRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTests()
        {
            var tests = await _testRepository.GetAllAsync(null, new List<int>(), new List<int>());
            return Ok(tests);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTestById(int id)
        {
            var test = await _testRepository.GetByIdAsync(id);
            return Ok(test);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTest([FromBody] CreateTestRequest request)
        {
            return Ok(new { message = "Тест создан", id = 1 });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTest(int id, [FromBody] UpdateTestRequest request)
        {
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTest(int id)
        {
            await _testRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}