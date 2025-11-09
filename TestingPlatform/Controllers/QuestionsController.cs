using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionsController : ControllerBase
    {
        //[HttpGet]
        //public IActionResult GetAllQuestions()
        //{
        //    return Ok("Список вопросов");
        //}

        //[HttpGet("{id:int}")]
        //public IActionResult GetQuestionById([FromRoute] int id)
        //{
        //    if (id <= 0) return BadRequest("Некорректный id"); //400
        //    if (id == 1) return Ok($"Вопрос {id}");
        //    return NotFound();
        //}

        //[HttpPost]
        //public IActionResult CreateQuestion()
        //{
        //    return Created("/api/questions/1/", "Создан вопрос с id = 1");
        //}

        //[HttpPut("{id:int}")]
        //public IActionResult UpdateQuestion([FromRoute] int id)
        //{
        //    if (id <= 0) return BadRequest("Некорректный id"); //400
        //    if (id != 1) return NotFound(); //404

        //    //Update...

        //    return NoContent(); //204
        //}

        //[HttpDelete("{id:int}")]
        //public IActionResult DeleteQuestion([FromRoute] int id)
        //{
        //    if (id <= 0) return BadRequest("Некорректный id"); //400
        //    if (id != 1) return NotFound(); //404

        //    //Delete

        //    return NoContent(); //204
        //}
    }
}
