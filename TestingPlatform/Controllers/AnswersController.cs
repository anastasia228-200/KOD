using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.domain.Models;
using TestingPlatform.Requests.Answer;
using TestingPlatform.Responses.Answer;

namespace TestingPlatform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnswersController : ControllerBase
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswersController(IAnswerRepository answerRepository, IMapper mapper)
        {
            _answerRepository = answerRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAnswers()
        {
            var answers = await _answerRepository.GetAllAsync();
            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return Ok(answerDtos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetAnswerById([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный id");

            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
                return NotFound();

            var answerDto = _mapper.Map<AnswerDto>(answer);
            return Ok(answerDto);
        }

        [HttpGet("question/{questionId:int}")]
        public async Task<IActionResult> GetAnswersByQuestionId([FromRoute] int questionId)
        {
            var answers = await _answerRepository.GetByQuestionIdAsync(questionId);
            var answerDtos = _mapper.Map<List<AnswerDto>>(answers);
            return Ok(answerDtos);
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnswer([FromBody] CreateAnswerRequest request)
        {
            var answer = _mapper.Map<Answer>(request);
            var id = await _answerRepository.CreateAsync(answer);

            var response = _mapper.Map<AnswerResponse>(answer);
            return Created($"/api/answers/{id}", response);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAnswer([FromRoute] int id, [FromBody] UpdateAnswerRequest request)
        {
            if (id <= 0)
                return BadRequest("Некорректный id");

            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
                return NotFound();

            _mapper.Map(request, answer);
            await _answerRepository.UpdateAsync(answer);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAnswer([FromRoute] int id)
        {
            if (id <= 0)
                return BadRequest("Некорректный id");

            var answer = await _answerRepository.GetByIdAsync(id);
            if (answer == null)
                return NotFound();

            await _answerRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}