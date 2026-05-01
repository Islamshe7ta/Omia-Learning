using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Entities;

namespace Omia.BLL.Helpers
{
    public static class DiscussionNotificationHelper
    {
        public static async Task CreateDiscussionNotificationsAsync(
            IUnitOfWork unitOfWork,
            string receiver,
            Guid courseId,
            Guid discussionId,
            Guid senderId,
            Guid teacherId)
        {
            if (string.IsNullOrWhiteSpace(receiver))
                return;

            var notificationUserIds = new List<Guid>();
            var lowerReceiver = receiver.ToLower().Trim();

            if (lowerReceiver == "group")
            {
                var studentIds = await unitOfWork.CourseStudents.GetStudentIdsByCourseAsync(courseId);
                notificationUserIds.AddRange(studentIds);

                notificationUserIds.Add(teacherId);

                var assistantIds = await unitOfWork.AssistantCourses.GetAssistantIdsByCourseAsync(courseId);
                notificationUserIds.AddRange(assistantIds);
            }
            else if (lowerReceiver == "teacher")
            {
                notificationUserIds.Add(teacherId);
            }
            else if (Guid.TryParse(receiver, out Guid specificUserId))
            {
                notificationUserIds.Add(specificUserId);
            }

            notificationUserIds = notificationUserIds
                .Where(id => id != senderId)
                .Distinct()
                .ToList();

            if (notificationUserIds.Count == 0)
                return;

            var notifications = notificationUserIds.Select(uid => new Notification
            {
                UserId = uid,
                Type = "DiscussionMessage",
                ReferenceId = discussionId,
                Title = "New question in course discussion"
            });

            await unitOfWork.Notifications.AddRangeAsync(notifications);
        }
    }
}
