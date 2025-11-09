using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.domain.Models;
using TestingPlatform.Requests.Test;
using TestingPlatform.Responses.Test;

namespace TestingPlatform.Mappings;

public class TestProfile : Profile
{
    public TestProfile()
    {
        // Model to DTO
        CreateMap<Test, TestDto>();

        // Request to Model
        CreateMap<CreateTestRequest, Test>();
        CreateMap<UpdateTestRequest, Test>();

        // Model to Response
        CreateMap<Test, TestResponse>();

        // DTO to Model and vice versa
        CreateMap<TestDto, Test>();
        CreateMap<Test, TestDto>();
    }
}