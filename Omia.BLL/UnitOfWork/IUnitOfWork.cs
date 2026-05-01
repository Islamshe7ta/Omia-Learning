using Omia.BLL.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IAdminRepository Admins { get; }
        IAssistantRepository Assistants { get; }
        IInstituteRepository Institutes { get; }
        IParentRepository Parents { get; }
        IStudentRepository Students { get; }
        ITeacherRepository Teachers { get; }
        IUserTokenRepository Tokens { get; }
        ICourseRepository Courses { get; }
        ICourseStudentRepository CourseStudents { get; }
        IAssistantCourseRepository AssistantCourses { get; }
        ICourseCategoryRepository CourseCategories { get; }
        ICourseContentRepository CourseContents { get; }
        ILiveSessionRepository LiveSessions { get; }
        IAssignmentRepository Assignments { get; }
        IAssignmentSubmissionRepository AssignmentSubmissions { get; }
        IQuizRepository Quizzes { get; }
        IQuizQuestionRepository QuizQuestions { get; }
        IQuizAttemptRepository QuizAttempts { get; }
        IQuizAnswerRepository QuizAnswers { get; }
        ICourseDiscussionRepository CourseDiscussions { get; }
        ICourseProgressRepository CourseProgresses { get; }
        INotificationRepository Notifications { get; }
        ICourseContentCommentRepository CourseContentComments { get; }
        IUserRepository Users { get; }

        Task<int> CommitAsync();
    }
}
