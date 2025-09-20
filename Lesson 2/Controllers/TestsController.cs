using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllTests()
        {
            return Ok("Список тестов");
        }

        [HttpGet("{id:int}")]
        public IActionResult GetTestById([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Некорректный id"); //400
            if (id == 1) return Ok($"Тест {id}");
            return NotFound();
        }

        [HttpPost]
        public IActionResult CreateTest()
        {
            return Created("/api/tests/1/", "Создан тест с id = 1");
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateTest([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Некорректный id"); //400
            if (id != 1) return NotFound(); //404

            //Update...

            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteTest([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Некорректный id"); //400
            if (id != 1) return NotFound(); //404

            //Delete

            return NoContent(); //204
        }
    }
}
