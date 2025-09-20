using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lesson_2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllSrudents()
        {
            return Ok("Список студентов");
        }

        [HttpGet("{id:int}")]
        public IActionResult GetStudentById([FromRoute] int id) 
        {
            if (id <= 0) return BadRequest("Некорректный id"); //400
            if (id == 1) return Ok($"Студент {id}");
            return NotFound();
        }

        [HttpPost]
        public IActionResult GetStudentBy()
        {
            return Created("/api/students/1/", "Создан студент с id = 1");
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent([FromRoute] int id)
        {
            if (id <= 0) return BadRequest("Некорректный id"); //400
            if (id != 1) return NotFound(); //404

            //Update...

            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent([FromRoute] int id) 
        {
            if (id <= 0) return BadRequest("Некорректный id"); //400
            if (id != 1) return NotFound(); //404

            //Delete

            return NoContent(); //204
        }
    }
}
