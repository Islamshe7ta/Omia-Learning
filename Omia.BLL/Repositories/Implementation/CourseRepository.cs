using Microsoft.EntityFrameworkCore;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using Omia.BLL.DTOs.Course;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseRepository : GenericRepository<Course>, ICourseRepository
    {
        private readonly OmiaDbContext _context;
        public CourseRepository(OmiaDbContext context) : base(context)
        {
            _context = context;

        }

        public async Task<IEnumerable<Course>> GetMyCourses(Guid userId)
        {
            return await _context.Courses
                .Where(c => c.CourseStudents.Any(cs => cs.StudentId == userId || cs.Student.ParentId == userId)
                                                || c.TeacherId == userId || c.InstituteId == userId
                                                || c.AssistantCourses.Any(ac => ac.AssistantId == userId))
                .Include(c => c.Teacher)
                .Include(c => c.Institute)
                .Include(c => c.AssistantCourses).ThenInclude(ac => ac.Assistant)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<Course?> GetFullCourseDetailsAsync(Guid courseId)
        {
            return await _context.Courses
                .Include(x => x.Teacher)
                .Include(x => x.Institute)
                .Include(x => x.AssistantCourses).ThenInclude(ac => ac.Assistant)
                .Include(x => x.Categories.OrderBy(c => c.OrderNumber))
                .Include(x => x.Contents.OrderBy(c => c.OrderNumber))
                .Include(x => x.LiveSessions.OrderBy(c => c.OrderNumber))
                .Include(x => x.Assignments.OrderBy(c => c.OrderNumber))
                .Include(x => x.Quizzes.OrderBy(c => c.OrderNumber))
                .Include(x => x.Discussions.OrderBy(d => d.CreatedAt)).ThenInclude(d => d.Sender)
                .Include(x => x.CourseStudents)
                .FirstOrDefaultAsync(x => x.Id == courseId);
        }

        public async Task<Course?> GetCourseBriefAsync(Guid courseId)
        {
            return await _context.Courses
                .Include(x => x.Teacher)
                .Include(x => x.Institute)
                .Include(x => x.AssistantCourses).ThenInclude(ac => ac.Assistant)
                .Include(x => x.Categories)
                .Include(x => x.Contents)
                .Include(x => x.LiveSessions)
                .Include(x => x.Assignments)
                .Include(x => x.Quizzes)
                .Include(x => x.Discussions)
                .Include(x => x.CourseStudents)
                .FirstOrDefaultAsync(x => x.Id == courseId);
        }

        public async Task<IEnumerable<NotificationDTO>> GetActivityFeedAsync(Guid courseId, Guid studentId, int limit = 50)
        {
            var course = await _context.Courses
                .Include(c => c.AssistantCourses)
                .FirstOrDefaultAsync(c => c.Id == courseId);

            if (course == null) return Enumerable.Empty<NotificationDTO>();

            var assistantIds = course.AssistantCourses.Select(ac => ac.AssistantId).ToList();

            var activities = new List<NotificationDTO>();


            var videos = await _context.CourseContents
                .Where(x => x.CourseId == courseId && x.ContentType == ContentType.Video)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new NotificationDTO
                {
                    Type = "NewVideo",
                    Title = x.Title,
                    ReferenceId = x.Id,
                    CreatedAt = x.CreatedAt,
                    AdditionalInformation = "New video uploaded"
                }).ToListAsync();
            activities.AddRange(videos);


            var assignments = await _context.Assignments
                .Where(x => x.CourseId == courseId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new NotificationDTO
                {
                    Type = "NewAssignment",
                    Title = x.Title,
                    ReferenceId = x.Id,
                    CreatedAt = x.CreatedAt,
                    AdditionalInformation = "New assignment posted"
                }).ToListAsync();
            activities.AddRange(assignments);


            var submissions = await _context.AssignmentSubmissions
                .Include(x => x.Assignment)
                .Where(x => x.Assignment.CourseId == courseId && x.StudentId == studentId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .ToListAsync();

            foreach (var sub in submissions)
            {
                activities.Add(new NotificationDTO
                {
                    Type = "AssignmentSubmitted",
                    Title = $"{sub.Assignment.Title} submitted",
                    ReferenceId = sub.Id,
                    CreatedAt = sub.CreatedAt
                });

                if (sub.Grade != null || sub.CorrectedBy != null)
                {
                    activities.Add(new NotificationDTO
                    {
                        Type = "AssignmentCorrected",
                        Title = $"{sub.Assignment.Title} corrected",
                        ReferenceId = sub.Id,
                        CreatedAt = sub.LastUpdatedAt,
                        AdditionalInformation = sub.TeacherComment
                    });
                }
            }

            var quizzes = await _context.Quizzes
                .Where(x => x.CourseId == courseId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new NotificationDTO
                {
                    Type = "NewQuiz",
                    Title = x.Title,
                    ReferenceId = x.Id,
                    CreatedAt = x.CreatedAt,
                    AdditionalInformation = "New quiz available"
                }).ToListAsync();
            activities.AddRange(quizzes);


            var attempts = await _context.QuizAttempts
                .Include(x => x.Quiz)
                .Where(x => x.Quiz.CourseId == courseId && x.StudentId == studentId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new NotificationDTO
                {
                    Type = "QuizAttempt",
                    Title = $"Quiz attempt: {x.Quiz.Title}",
                    ReferenceId = x.Id,
                    CreatedAt = x.CreatedAt
                }).ToListAsync();
            activities.AddRange(attempts);


            var sessions = await _context.LiveSessions
                .Where(x => x.CourseId == courseId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .Select(x => new NotificationDTO
                {
                    Type = "NewLiveSession",
                    Title = x.Title,
                    ReferenceId = x.Id,
                    CreatedAt = x.CreatedAt,
                    AdditionalInformation = $"Starts at {x.StartTime}"
                }).ToListAsync();
            activities.AddRange(sessions);


            var discussions = await _context.CourseDiscussions
                .Include(x => x.Sender)
                .Where(x => x.CourseId == courseId)
                .OrderByDescending(x => x.CreatedAt)
                .Take(limit)
                .ToListAsync();

            foreach (var disc in discussions)
            {
                bool isReply = disc.SenderId == course.TeacherId || assistantIds.Contains(disc.SenderId);
                activities.Add(new NotificationDTO
                {
                    Type = isReply ? "DiscussionReply" : "DiscussionMessage",
                    Title = isReply ? $"Teacher replied: {disc.Message.Substring(0, Math.Min(20, disc.Message.Length))}..." : $"New message: {disc.Message.Substring(0, Math.Min(20, disc.Message.Length))}...",
                    ReferenceId = disc.Id,
                    CreatedAt = disc.CreatedAt,
                    AdditionalInformation = $"From {disc.Sender.FullName}"
                });
            }

            return activities.OrderByDescending(x => x.CreatedAt).Take(limit).ToList();
        }
    }
}
