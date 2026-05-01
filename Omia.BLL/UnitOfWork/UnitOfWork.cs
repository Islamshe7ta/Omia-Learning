using Omia.BLL.Repositories.Implementation;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using System;
using System.Threading.Tasks;

namespace Omia.BLL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly OmiaDbContext _context;
        private IAdminRepository? _admins;
        private IAssistantRepository? _assistants;
        private IInstituteRepository? _institutes;
        private IParentRepository? _parents;
        private IStudentRepository? _students;
        private ITeacherRepository? _teachers;
        private IUserTokenRepository? _tokens;
        private ICourseRepository? _courses;
        private ICourseStudentRepository? _courseStudents;
        private IAssistantCourseRepository? _assistantCourses;
        private ICourseCategoryRepository? _courseCategories;
        private ICourseContentRepository? _courseContents;
        private ILiveSessionRepository? _liveSessions;
        private IAssignmentRepository? _assignments;
        private IAssignmentSubmissionRepository? _assignmentSubmissions;
        private IQuizRepository? _quizzes;
        private IQuizQuestionRepository? _quizQuestions;
        private IQuizAttemptRepository? _quizAttempts;
        private IQuizAnswerRepository? _quizAnswers;
        private ICourseDiscussionRepository? _courseDiscussions;
        private ICourseProgressRepository? _courseProgresses;
        private INotificationRepository? _notifications;
        private ICourseContentCommentRepository? _courseContentComments;
        private IUserRepository? _users;

        public UnitOfWork(OmiaDbContext context)
        {
            _context = context;
        }

        public IAdminRepository Admins => _admins ??= new AdminRepository(_context);
        public IAssistantRepository Assistants => _assistants ??= new AssistantRepository(_context);
        public IInstituteRepository Institutes => _institutes ??= new InstituteRepository(_context);
        public IParentRepository Parents => _parents ??= new ParentRepository(_context);
        public IStudentRepository Students => _students ??= new StudentRepository(_context);
        public ITeacherRepository Teachers => _teachers ??= new TeacherRepository(_context);
        public IUserTokenRepository Tokens => _tokens ??= new UserTokenRepository(_context);
        public ICourseRepository Courses => _courses ??= new CourseRepository(_context);
        public ICourseStudentRepository CourseStudents => _courseStudents ??= new CourseStudentRepository(_context);
        public IAssistantCourseRepository AssistantCourses => _assistantCourses ??= new AssistantCourseRepository(_context);
        public ICourseCategoryRepository CourseCategories => _courseCategories ??= new CourseCategoryRepository(_context);
        public ICourseContentRepository CourseContents => _courseContents ??= new CourseContentRepository(_context);
        public ILiveSessionRepository LiveSessions => _liveSessions ??= new LiveSessionRepository(_context);
        public IAssignmentRepository Assignments => _assignments ??= new AssignmentRepository(_context);
        public IAssignmentSubmissionRepository AssignmentSubmissions => _assignmentSubmissions ??= new AssignmentSubmissionRepository(_context);
        public IQuizRepository Quizzes => _quizzes ??= new QuizRepository(_context);
        public IQuizQuestionRepository QuizQuestions => _quizQuestions ??= new QuizQuestionRepository(_context);
        public IQuizAttemptRepository QuizAttempts => _quizAttempts ??= new QuizAttemptRepository(_context);
        public IQuizAnswerRepository QuizAnswers => _quizAnswers ??= new QuizAnswerRepository(_context);
        public ICourseDiscussionRepository CourseDiscussions => _courseDiscussions ??= new CourseDiscussionRepository(_context);
        public ICourseProgressRepository CourseProgresses => _courseProgresses ??= new CourseProgressRepository(_context);
        public INotificationRepository Notifications => _notifications ??= new NotificationRepository(_context);
        public ICourseContentCommentRepository CourseContentComments => _courseContentComments ??= new CourseContentCommentRepository(_context);
        public IUserRepository Users => _users ??= new UserRepository(_context);

        public async Task<int> CommitAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
