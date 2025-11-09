using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TestingPlatform.Application.Dtos;
using TestingPlatform.Application.Interfaces;
using TestingPlatform.domain.Models;
using TestingPlatform.Infrastructure.Repositories;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

namespace TestingPlatform.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestsController(TestRepository testRepository) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> info(int id) 
        {
            var t = await testRepository.GetTestByTypeAsync();
            return Ok();
        }
    }
}