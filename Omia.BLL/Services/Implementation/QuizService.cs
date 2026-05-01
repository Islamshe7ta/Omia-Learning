using AutoMapper;
using Omia.BLL.DTOs;
using Omia.BLL.DTOs.Quiz;
using Omia.BLL.DTOs.QuizAttempt;
using Omia.BLL.DTOs.QuizQuestion;
using Omia.BLL.Helpers;
using Omia.BLL.Services.Interfaces;
using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;

namespace Omia.BLL.Services.Implementation
{
    public class QuizService : IQuizService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public QuizService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<QuizResponseDTO> CreateQuizAsync(CreateQuizDTO createQuizDTO, Guid userId)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(createQuizDTO.CourseId);
            if (course == null)
            {
                return new QuizResponseDTO { IsSuccess = false, Message = "Course not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork, userId, createQuizDTO.CourseId, AssistantPermissions.QuizManager);

            if (!hasPermission)
            {
                return new QuizResponseDTO { IsSuccess = false, Message = "You don't have permission to create quizzes for this course" };
            }

            if (createQuizDTO.CategoryId.HasValue)
            {
                var isCategoryValid = await _unitOfWork.CourseCategories.IsCategoryInCourseAsync(createQuizDTO.CategoryId.Value, createQuizDTO.CourseId);
                if (!isCategoryValid)
                {
                    return new QuizResponseDTO { IsSuccess = false, Message = "Category not found or doesn't belong to this course" };
                }
            }

            var quiz = _mapper.Map<Quiz>(createQuizDTO);
            quiz.CreatedBy = userId;
            quiz.CreatedAt = DateTime.UtcNow;

            if (!createQuizDTO.OrderNumber.HasValue)
            {
                var maxOrder = await _unitOfWork.Quizzes.MaxAsync(q => q.CategoryId == createQuizDTO.CategoryId && q.CourseId == createQuizDTO.CourseId, q => q.OrderNumber);
                quiz.OrderNumber = maxOrder + 1;
            }

            await _unitOfWork.Quizzes.AddAsync(quiz);
            await _unitOfWork.CommitAsync();

            return new QuizResponseDTO
            {
                IsSuccess = true,
                Message = "Quiz created successfully",
                QuizId = quiz.Id,
                CreatedAt = quiz.CreatedAt
            };
        }


        public async Task<BaseResponseDTO> UpdateQuizAsync(Guid QuizId, UpdateQuizDTO updateQuizDTO, Guid userId)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(QuizId);
            if (quiz == null) return new BaseResponseDTO { IsSuccess = false, Message = "Quiz not found" };

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork, userId, quiz.CourseId, AssistantPermissions.QuizManager);

            if (!hasPermission) return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to update quizzes" };

            if (updateQuizDTO.CategoryId.HasValue)
            {
                var isCategoryValid = await _unitOfWork.CourseCategories.IsCategoryInCourseAsync(updateQuizDTO.CategoryId.Value, quiz.CourseId);
                if (!isCategoryValid)
                {
                    return new BaseResponseDTO { IsSuccess = false, Message = "Category not found or doesn't belong to this course" };
                }
            }

            if (updateQuizDTO.Questions != null && updateQuizDTO.Questions.Any())
            {
                await _unitOfWork.QuizQuestions.DeleteQuestionsByQuizIdAsync(QuizId);

                foreach (var questionDto in updateQuizDTO.Questions)
                {
                    var newQuestion = _mapper.Map<QuizQuestion>(questionDto);
                    newQuestion.QuizId = QuizId;
                    await _unitOfWork.QuizQuestions.AddAsync(newQuestion);
                }
            }

            var originalOrder = quiz.OrderNumber;
            _mapper.Map(updateQuizDTO, quiz);

            if (!updateQuizDTO.OrderNumber.HasValue)
            {
                quiz.OrderNumber = originalOrder;
            }

            _unitOfWork.Quizzes.Update(quiz);

            await _unitOfWork.CommitAsync();

            return new BaseResponseDTO { IsSuccess = true, Message = "Quiz updated successfully" };
        }

        public async Task<BaseResponseDTO> DeleteQuizAsync(Guid quizId, Guid userId)
        {

            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(quizId);
            if (quiz == null) return new BaseResponseDTO { IsSuccess = false, Message = "quiz not found" };

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(_unitOfWork, userId, quiz.CourseId, AssistantPermissions.QuizManager);
            if (!hasPermission) return new BaseResponseDTO { IsSuccess = false, Message = "You are not allowed to delete quizzes" };

            _unitOfWork.Quizzes.Delete(quiz);
            await _unitOfWork.CommitAsync();
            return new BaseResponseDTO { IsSuccess = true, Message = "quiz deleted successfully" };

        }

        public async Task<QuizDetailsResponseDTO> GetQuizDetailsAsync(Guid quizId, Guid userId)
        {
            var quiz = await _unitOfWork.Quizzes.GetQuizWithQuestionsAsync(quizId);

            if (quiz == null)
            {
                return new QuizDetailsResponseDTO { IsSuccess = false, Message = "quiz not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(_unitOfWork, userId, quiz.CourseId, AssistantPermissions.QuizManager);
            if (!hasPermission)
            {
                return new QuizDetailsResponseDTO { IsSuccess = false, Message = "You are not allowed to get quiz details" };
            }

            var quizDetails = _mapper.Map<QuizDetailsDTO>(quiz);

            return new QuizDetailsResponseDTO
            {
                IsSuccess = true,
                Message = "Quiz details retrieved successfully",
                Data = quizDetails
            };
        }

        public async Task<IEnumerable<QuizzesCourseResponse>> GetQuizzesByCourseIdAsync(Guid courseId, Guid userId)
        {
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, userId, courseId);

            if (!hasAccess)
            {
                return Enumerable.Empty<QuizzesCourseResponse>();
            }

            var quizzes = await _unitOfWork.Quizzes.GetQuizzesByCourseIdAsync(courseId);

            return _mapper.Map<IEnumerable<QuizzesCourseResponse>>(quizzes);
        }

        public async Task<StartQuizResponseDTO> StartQuizAsync(StartQuizRequestDTO request, Guid studentId)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(request.QuizId);
            if (quiz == null)
            {
                return new StartQuizResponseDTO { IsSuccess = false, Message = "Quiz not found" };
            }

            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, studentId, quiz.CourseId);
            if (!hasAccess)
            {
                return new StartQuizResponseDTO { IsSuccess = false, Message = "Student not enrolled in this course" };
            }

            var now = DateTime.UtcNow;
            if (quiz.StartDate.HasValue && now < quiz.StartDate.Value)
            {
                return new StartQuizResponseDTO { IsSuccess = false, Message = "Quiz not available yet" };
            }

            if (quiz.EndDate.HasValue && now > quiz.EndDate.Value)
            {
                return new StartQuizResponseDTO { IsSuccess = false, Message = "Quiz time has ended" };
            }

            var existingAttempt = await _unitOfWork.QuizAttempts.GetActiveAttemptAsync(request.QuizId, studentId);
            if (existingAttempt != null)
            {
                return new StartQuizResponseDTO { IsSuccess = false, Message = "Quiz already started" };
            }

            var attempt = new QuizAttempt
            {
                QuizId = request.QuizId,
                StudentId = studentId,
                StartTime = now,
                Status = QuizAttemptStatus.InProgress
            };

            await _unitOfWork.QuizAttempts.AddAsync(attempt);
            await _unitOfWork.CommitAsync();

           
            attempt.Quiz = quiz;

            var attemptDto = _mapper.Map<QuizAttemptStartDTO>(attempt);

            return new StartQuizResponseDTO
            {
                IsSuccess = true,
                Message = "Quiz started successfully",
                Attempt = attemptDto
            };
        }

        public async Task<EndQuizResponseDTO> EndQuizAsync(EndQuizRequestDTO request, Guid studentId)
        {
            var attempt = await _unitOfWork.QuizAttempts.GetAttemptWithQuizAndQuestionsAsync(request.AttemptId);
            if (attempt == null || attempt.StudentId != studentId)
            {
                return new EndQuizResponseDTO { IsSuccess = false, Message = "Quiz attempt not found" };
            }

            var validation = QuizHelper.ValidateAttemptStatus(attempt);
            if (!validation.IsValid) 
            {
                return new EndQuizResponseDTO { IsSuccess = false, Message = validation.Message };
            }

            var results = QuizHelper.ProcessAndGradeAnswers(attempt, request.Answers);

            await _unitOfWork.QuizAnswers.AddRangeAsync(results.QuizAnswers);

            attempt.EndTime = DateTime.UtcNow;
            attempt.Status = QuizAttemptStatus.Completed;
            attempt.Score = results.TotalScore;

            _unitOfWork.QuizAttempts.Update(attempt);
            await _unitOfWork.CommitAsync();

            return new EndQuizResponseDTO
            {
                IsSuccess = true,
                Message = "Quiz submitted successfully",
                Result = new QuizResultDTO
                {
                    AttemptId = attempt.Id,
                    Score = (float)Math.Round(attempt.Score ?? 0, 2),
                    TotalMarks = attempt.Quiz.TotalMarks ?? 0,
                    Status = attempt.Status.ToString(),
                    StartTime = attempt.StartTime,
                    EndTime = attempt.EndTime
                }
            };
        }
 
        public async Task<IEnumerable<MyQuizAttemptDTO>> GetMyAttemptsAsync(Guid quizId, Guid studentId)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return Enumerable.Empty<MyQuizAttemptDTO>();
            }
 
            var hasAccess = await CourseAccessHelper.CheckUserCourseAccessAsync(_unitOfWork, studentId, quiz.CourseId);
            if (!hasAccess)
            {
                return Enumerable.Empty<MyQuizAttemptDTO>();
            }
 
            var attempts = await _unitOfWork.QuizAttempts.GetAttemptsByStudentAndQuizAsync(quizId, studentId);
 
            return _mapper.Map<IEnumerable<MyQuizAttemptDTO>>(attempts);
        }

        public async Task<GetQuizAttemptsForTeacherResponseDTO> GetQuizAttemptsForTeacherAsync(Guid quizId, Guid userId)
        {
            var quiz = await _unitOfWork.Quizzes.GetByIdAsync(quizId);
            if (quiz == null)
            {
                return new GetQuizAttemptsForTeacherResponseDTO { IsSuccess = false, Message = "Quiz not found" };
            }

            var hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                _unitOfWork, userId, quiz.CourseId, AssistantPermissions.QuizManager);

            if (!hasPermission)
            {
                return new GetQuizAttemptsForTeacherResponseDTO { IsSuccess = false, Message = "You don't have permission to view attempts for this quiz" };
            }

            var attempts = await _unitOfWork.QuizAttempts.GetAllAttemptsByQuizIdAsync(quizId);

            return new GetQuizAttemptsForTeacherResponseDTO
            {
                IsSuccess = true,
                Attempts = _mapper.Map<IEnumerable<QuizAttemptTeacherDTO>>(attempts)
            };
        }

        public async Task<QuizAttemptDetailsResponseDTO> GetQuizAttemptDetailsAsync(Guid attemptId, Guid userId)
        {
            var attempt = await _unitOfWork.QuizAttempts.GetAttemptWithAllDetailsAsync(attemptId);
            if (attempt == null)
            {
                return new QuizAttemptDetailsResponseDTO { IsSuccess = false, Message = "Quiz attempt not found" };
            }

            // Check permissions: Teacher/Assistant of the course OR the Student who owns the attempt
            bool hasPermission = attempt.StudentId == userId;

            if (!hasPermission)
            {
                hasPermission = await CourseAccessHelper.CheckUserPermissionAsync(
                    _unitOfWork, userId, attempt.Quiz.CourseId, AssistantPermissions.QuizManager);
            }

            if (!hasPermission)
            {
                return new QuizAttemptDetailsResponseDTO { IsSuccess = false, Message = "You don't have permission to view these attempt details" };
            }

            return new QuizAttemptDetailsResponseDTO
            {
                IsSuccess = true,
                Data = _mapper.Map<QuizAttemptDetailsDTO>(attempt)
            };
        }

    }
}
