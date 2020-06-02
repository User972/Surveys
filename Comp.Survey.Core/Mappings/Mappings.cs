using System;
using AutoMapper;
using Comp.Survey.Core.Entities;
using Comp.Survey.Core.Interfaces.DTO;

// ReSharper disable once CheckNamespace
namespace Comp.Survey.Core
{
    public static class Mappings
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg => {
                // This line ensures that internal properties are also mapped over.
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<MappingProfile>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });

        public static IMapper Mapper => Lazy.Value;
    }
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ISurvey, Entities.Survey>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ISurveyQuestion, SurveyQuestion>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                .ForMember(dest => dest.SurveyId, opt => opt.Ignore())
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => src.CreatedDateTime))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                .ForMember(dest => dest.SubTitle, opt => opt.MapFrom(src => src.SubTitle))
                .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                .ReverseMap()
                .ForPath(dest => dest.Id, opt => opt.MapFrom(src=>src.Id));

            CreateMap<IQuestionOption, QuestionOption>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                .ForMember(dest => dest.SurveyQuestionId, opt => opt.Ignore())
                .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ICompUser, CompUser>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ICompUserSurvey, CompUserSurvey>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                //.ForMember(dest => dest.CompUserSurveyDetails, opt => opt.Ignore())
                .ForMember(dest => dest.SurveyId, opt => opt.MapFrom(src => src.SurveyId))
                .ForMember(dest => dest.CompUserId, opt => opt.MapFrom(src => src.CompUserId))
                .ForMember(dest => dest.SubmissionTitle, opt => opt.MapFrom(src => src.SubmissionTitle))
                
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            CreateMap<ICompUserSurveyDetail, CompUserSurveyDetail>()
                .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                .ForMember(dest => dest.SurveyQuestionId, opt => opt.MapFrom(src => src.SurveyQuestionId))
                .ForMember(dest => dest.CompUserSurveyId, opt => opt.MapFrom(src => src.CompUserSurveyId))
                .ForMember(dest => dest.SelectedOptionId, opt => opt.MapFrom(src => src.SelectedOptionId))
                .ForMember(dest => dest.SelectedOptionValue, opt => opt.MapFrom(src => src.SelectedOptionValue))

                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));
        }
    }
}
