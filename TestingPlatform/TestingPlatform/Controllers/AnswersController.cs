using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnswersController : ControllerBase
    {
        // GET api/answers
        [HttpGet]
        public IActionResult GetAllAnswers() => Ok("Список всех ответов");

        // GET api/answers/{id}
        [HttpGet("{id}")]
        public IActionResult GetAnswerById(int id)
        {
            if (id == 1)
                return Ok("Ответ 1");
            return NotFound();
        }

        // GET api/answers/by-question/{questionId}
        [HttpGet("by-question/{questionId}")]
        public IActionResult GetAnswersByQuestionId(int questionId) =>
            Ok($"Ответы для вопроса {questionId}");

        // POST api/answers
        [HttpPost]
        public IActionResult CreateAnswer() => Created("/api/answers/1", "Ответ создан");

        // PUT api/answers/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAnswer(int id) => NoContent();

        // DELETE api/answers/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAnswer(int id) => NoContent();
    }
}
