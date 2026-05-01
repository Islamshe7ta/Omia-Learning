using Omia.DAL.Data;
using Omia.DAL.Models.Entities;
using Omia.BLL.Repositories.Implementation.Base;
using Omia.BLL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Omia.BLL.Repositories.Implementation
{
    public class CourseDiscussionRepository : GenericRepository<CourseDiscussion>, ICourseDiscussionRepository
    {
        private readonly OmiaDbContext _context;
        public CourseDiscussionRepository(OmiaDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CourseDiscussion>> GetDiscussionsByCourseAsync(Guid courseId)
        {
            return await _context.CourseDiscussions
                .Where(d => d.CourseId == courseId && d.Receiver == "group")
                .Include(d => d.Sender)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Omia.BLL.DTOs.CourseDiscussion.ChatHomeItemDTO>> GetChatHomeItemsAsync(Guid userId)
        {
            var userIdStr = userId.ToString();

            var baseData = await _context.Courses
                .Where(c =>
                    c.TeacherId == userId || // include ALL teacher courses
                    c.CourseStudents.Any(cs => cs.StudentId == userId) ||
                    c.AssistantCourses.Any(ac => ac.AssistantId == userId))
                .Select(c => new
                {
                    CourseId = c.Id,
                    CourseName = c.Name,
                    CourseImage = c.Image,

                    // count only relevant messages (will be 0 if none)
                    TotalMessages = c.Discussions
                        .Count(d => d.SenderId == userId || d.Receiver == userIdStr),

                    // may be null (important for empty chats)
                    LatestDiscussion = c.Discussions
                        .Where(d => d.SenderId == userId || d.Receiver == userIdStr)
                        .OrderByDescending(d => d.CreatedAt)
                        .FirstOrDefault()
                })
                .ToListAsync();

            var latestDiscussionIds = baseData
                .Where(b => b.LatestDiscussion != null)
                .Select(b => b.LatestDiscussion!.Id)
                .ToList();

            var senders = await _context.CourseDiscussions
                .Where(d => latestDiscussionIds.Contains(d.Id))
                .Include(d => d.Sender)
                .Select(d => new { d.Id, d.Sender })
                .ToDictionaryAsync(d => d.Id, d => d.Sender);

            return baseData.Select(b =>
            {
                var sender = b.LatestDiscussion != null &&
                             senders.TryGetValue(b.LatestDiscussion.Id, out var s)
                                ? s
                                : null;

                return new Omia.BLL.DTOs.CourseDiscussion.ChatHomeItemDTO
                {
                    CourseId = b.CourseId,
                    CourseName = b.CourseName ?? string.Empty,
                    CourseImage = b.CourseImage,

                    // null for empty chats
                    LastMessage = b.LatestDiscussion?.Message,
                    LastMessageAt = b.LatestDiscussion?.CreatedAt,

                    // 0 for empty chats
                    TotalMessages = b.TotalMessages,

                    LastMessageSender = sender != null
                        ? new Omia.BLL.DTOs.CourseDiscussion.SenderProfileDTO
                        {
                            Id = sender.Id,
                            FullName = sender.FullName ?? string.Empty,
                            ProfileImageUrl = sender.ProfileImageUrl,
                            Subtitle = Omia.BLL.Helpers.UserRoleHelper.GetUserRole(sender)
                        }
                        : null
                };
            })
            // ensures empty chats go to bottom
            .OrderByDescending(x => x.LastMessageAt ?? DateTime.MinValue)
            .ToList();
        }

        public async Task<List<CourseDiscussion>> GetPrivateChatAsync(Guid currentUserId, Guid otherUserId)
        {
            var otherUserIdStr = otherUserId.ToString();
            var currentUserIdStr = currentUserId.ToString();

            return await _context.CourseDiscussions
                .Include(d => d.Sender)
                .Where(d =>
                    (d.SenderId == currentUserId && d.Receiver == otherUserIdStr) ||
                    (d.SenderId == otherUserId && d.Receiver == currentUserIdStr)
                )
                .OrderBy(d => d.CreatedAt)
                .ToListAsync();
        }
    }
}
