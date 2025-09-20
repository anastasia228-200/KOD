using Microsoft.AspNetCore.Mvc;

namespace Lesson_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        //    [HttpGet("check-header")]
        //    public string CheckHeader([FromHeader(Name = "User-Agent")] string userAgent)
        //    {
        //        return $"Ваш User-Agent: {userAgent}";
        //    }

        //    [HttpGet("search")]
        //    public string Search([FromQuery] string query, [FromQuery] int page = 1)
        //    {
        //        return $"Поиск: {query}, страница {page}";
        //    }

        //    [HttpPost("add-user")]
        //    public string AddUser([FromBody] UserDto user)
        //    {
        //        return $"Добавлен пользователь {user.Name}, возраст {user.Age}";
        //    }
        //}

        //public class UserDto
        //{
        //    public string Name { get; set; }
        //    public string Age { get; set; }
        //}

        [HttpGet("{days}")]
        public IEnumerable<WeatherForecast> GetWithDays([FromRoute] int days)
        {
            return Enumerable.Range(1, days).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)), 
                TemperatureC = Random.Shared.Next(-22,55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
        }

        [HttpGet("filter")]
        public IEnumerable<WeatherForecast> GetFiltered([FromQuery] int minTemp, [FromQuery] int maxTemp)
        {
            var data = Enumerable.Range(1, 20).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-22, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
            return data.Where(w => w.TemperatureC >= minTemp && w.TemperatureC <= maxTemp);
        }
        [HttpGet("check-header")]
        public string CheckHeader([FromHeader(Name = "X-Client")] string client)
        {
            Response.Headers["X-Echo-Client"] = client ?? "unkhow";
            return $"Запрос пришел от клиента: {client}";
        }
    }
    public class WeatherForecast
    {
        public DateOnly Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string? Summary { get; set; }
    }
}


