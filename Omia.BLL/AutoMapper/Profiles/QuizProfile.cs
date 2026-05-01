using AutoMapper;
using Omia.BLL.DTOs.Quiz;
using Omia.BLL.DTOs.QuizAttempt;
using Omia.BLL.DTOs.QuizQuestion;
using Omia.BLL.DTOs.Student;
using Omia.DAL.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.AutoMapper.Profiles
{
    public class QuizProfile : Profile
    {
        public QuizProfile()
        {
            CreateMap<CreateQuizDTO, Quiz>();

            CreateMap<CreateQuizQuestionDTO, QuizQuestion>();
            CreateMap<UpdateQuizDTO, Quiz>()
                .ForMember(dest => dest.Questions, opt => opt.Ignore());

            CreateMap<Quiz, QuizDetailsDTO>();
            CreateMap<QuizQuestion, QuizQuestionDetailsDTO>()
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType.ToString()));

            CreateMap<Quiz, QuizzesCourseResponse>()
                .ForMember(dest => dest.StartTime, opt => opt.MapFrom(src => src.StartDate))
                .ForMember(dest => dest.EndTime, opt => opt.MapFrom(src => src.EndDate));

            CreateMap<QuizAttempt, QuizAttemptStartDTO>()
                .ForMember(dest => dest.DurationMinutes, opt => opt.MapFrom(src => src.Quiz.DurationMinutes ?? 0))
                .ForMember(dest => dest.EndTimeAllowed, opt => opt.MapFrom(src => src.StartTime.AddMinutes(src.Quiz.DurationMinutes ?? 0)));
 
            CreateMap<QuizAttempt, MyQuizAttemptDTO>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.AttemptAt, opt => opt.MapFrom(src => src.StartTime))
                .ForMember(dest => dest.QuizQuestions, opt => opt.MapFrom(src => src.Quiz.Questions))
                .ForMember(dest => dest.QuizAnswers, opt => opt.MapFrom(src => src.Answers));
 
            CreateMap<QuizQuestion, MyQuizAttemptQuestionDTO>()
                .ForMember(dest => dest.QuestionType, opt => opt.MapFrom(src => src.QuestionType.ToString()));
 
            CreateMap<QuizAnswer, MyQuizAttemptAnswerDTO>();
 
            CreateMap<QuizAttempt, QuizAttemptTeacherDTO>();
 
            CreateMap<Student, StudentBriefDTO>();
  
            CreateMap<QuizAttempt, QuizAttemptDetailsDTO>();
            CreateMap<QuizAnswer, QuizAnswerDetailsDTO>()
                .ForMember(dest => dest.QuestionText, opt => opt.MapFrom(src => src.Question.QuestionText))
                .ForMember(dest => dest.CorrectAnswer, opt => opt.MapFrom(src => src.Question.CorrectAnswer));
        }
    }
}
