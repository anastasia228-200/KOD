using AutoMapper;
using TestingPlatform.Application.Dtos;
using TestingPlatform.domain.Models;
using TestingPlatform.Requests.Answer;
using TestingPlatform.Responses.Answer;

namespace TestingPlatform.Mappings;

public class AnswerProfile : Profile
{
    public AnswerProfile()
    {
        // Model to DTO
        CreateMap<Answer, AnswerDto>();

        // Request to Model
        CreateMap<CreateAnswerRequest, Answer>();
        CreateMap<UpdateAnswerRequest, Answer>();

        // Model to Response
        CreateMap<Answer, AnswerResponse>();

        // DTO to Model and vice versa
        CreateMap<AnswerDto, Answer>();
        CreateMap<Answer, AnswerDto>();
    }
}