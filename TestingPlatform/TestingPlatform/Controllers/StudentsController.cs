using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace TestingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAllStudents() => Ok("Список студентов"); // 200 

        [HttpGet("{id:int}")]
        public IActionResult GetStudentById(int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный id"); // 400 
            if (id == 1)
                return Ok("Студент 1"); // 200 (заглушка) 
            return NotFound(); // 404
        }

        [HttpPost]
        public IActionResult CreateStudent()
        {
            // Имитация: создали студента с id=1 
            return Created("/api/students/1", "Создан студент с id=1"); // 201 + Location
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateStudent(int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный id"); // 400 
                                                      // Имитация: если не существует 
            if (id != 1)
                return NotFound(); // 404 
                                   // Имитация: обновили 
            return NoContent(); // 204 
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteStudent(int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный id"); // 400 
            if (id != 1) return NotFound(); // 404 
            return NoContent(); // 204 
        }
    }
}
