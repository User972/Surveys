using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Comp.Survey.App.Models;
using Comp.Survey.Core.Interfaces.DTO;

namespace Comp.Survey.App.Mappings
{
    public class Mappings
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
                CreateMap<SurveyCreationRequest, Models.Survey>()
                    .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                    .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));


                CreateMap<SurveyQuestionModel, ISurveyQuestion>()
                    .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                    .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                    .ForMember(dest => dest.CreatedDateTime, opt => opt.MapFrom(src => src.CreatedDateTime))
                    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy))
                    .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType))
                    .ForMember(dest => dest.SubTitle, opt => opt.MapFrom(src => src.SubTitle))
                    .ForMember(dest => dest.CreatedBy, opt => opt.MapFrom(src => src.CreatedBy));
                   

                CreateMap<IQuestionOption, QuestionOption>()
                    .ForMember(dest => dest.Id, opt => Guid.NewGuid())
                    .ForMember(dest => dest.Text, opt => opt.MapFrom(src => src.Text))
                    .ReverseMap()
                    .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id));

            }
        }
    }