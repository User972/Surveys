using System;
using System.Collections.Generic;
using System.Linq;
using Comp.Survey.Core.Entities;

namespace Comp.Survey.Infrastructure.Tests.MockData
{
    public static class MockSurveys
    {
        public static Guid GalaxyGuid => new Guid("12345678-52cb-4db5-a294-75f85eadd611");
        public static Guid XiomiGuid => new Guid("22345678-52cb-4db5-a294-75f85eadd611");
        public static Guid iPhoneGuid => new Guid("32345678-52cb-4db5-a294-75f85eadd611");

        public static Core.Entities.Survey GalaxyS10 =>
            new Core.Entities.Survey
            {
                Id = GalaxyGuid,
                Name = "Samsung Galaxy S10",
                SurveyQuestions = new List<SurveyQuestion>{
                    new SurveyQuestion
                    {
                        Id = Guid.NewGuid(),
                        SurveyId = GalaxyGuid,
                        Title = "Is White galaxy good?",
                        SubTitle = "White Samsung Galaxy S10",
                        QuestionOptions = new List<QuestionOption>()
                        {
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text1"
                            }
                        }
                    },
                    new SurveyQuestion
                    {
                        Id= Guid.NewGuid(),
                        SurveyId =GalaxyGuid,
                        Title =  "Black color is cool ?",
                        SubTitle=  "Black Samsung Galaxy S10",
                        QuestionOptions = new List<QuestionOption>()
                        {
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text2"
                            },
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text3"
                            }
                        }
                    }
                }
            };

        public static Core.Entities.Survey Xiomi =>
            new Core.Entities.Survey
            {
                Id = XiomiGuid,
                Name = "Xiomi Mi 10 Pro",
                SurveyQuestions = new List<SurveyQuestion>{
                    new SurveyQuestion
                    {
                        Id = Guid.NewGuid(),
                        SurveyId = XiomiGuid,
                        Title = "is White xiomi good?",
                        SubTitle = "White Xiomi",
                        QuestionOptions = new List<QuestionOption>()
                        {
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text4"
                            },
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text5"
                            }
                        }
                    },
                    new SurveyQuestion
                    {
                        Id= Guid.NewGuid(),
                        SurveyId =XiomiGuid,
                        Title =  "is Black xiomi any cooler than white?",
                        SubTitle=  "Black Xiomi",
                        QuestionOptions = new List<QuestionOption>()
                        {
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text6"
                            },
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text7"
                            }
                        }
                    }
                }
            };
        public static Core.Entities.Survey iPhonePro =>
            new Core.Entities.Survey
            {
                Id = iPhoneGuid,
                Name = "iPhone Pro",
                SurveyQuestions = new List<SurveyQuestion> {
                    new SurveyQuestion
                    {
                        Id = Guid.NewGuid(),
                        SurveyId = iPhoneGuid,
                        Title =  "Super ultra White iphone is new hip?",
                        SubTitle = "White iPhone Pro",
                        QuestionOptions = new List<QuestionOption>()
                        {
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text8"
                            },
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text9"
                            }
                        }
                    },
                    new SurveyQuestion
                    {
                        Id= Guid.NewGuid(),
                        SurveyId = iPhoneGuid,
                        Title = "Do you mind buying a standard Black iphone?",
                        SubTitle =  "Black iPhone Pro",
                        QuestionOptions = new List<QuestionOption>()
                        {
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text10"
                            },
                            new QuestionOption()
                            {
                                Id = Guid.NewGuid(),
                                Text = "text11"
                            }
                        }
                    } }
            };

        public static SurveyQuestion NewQuestion => new SurveyQuestion
        {
            Id = Guid.NewGuid(),
            Title = "some name",
            SubTitle = "some desc"
        };

        public static IEnumerable<Core.Entities.Survey> AllSurveys =>
            new List<Core.Entities.Survey>
            {
                GalaxyS10,
                Xiomi,
                iPhonePro
            };

        public static IEnumerable<Core.Entities.Survey> SurveysWithoutOptions =>
            AllSurveys.Select(p =>
            {
                p.SurveyQuestions = new List<SurveyQuestion>();
                return p;
            });

        public static IEnumerable<Core.Entities.Survey> SurveysWithNullOptions =>
            AllSurveys.Select(p =>
            {
                p.SurveyQuestions = null;
                return p;
            });
    }
}
