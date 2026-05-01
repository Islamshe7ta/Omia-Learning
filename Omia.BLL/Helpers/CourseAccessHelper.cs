using Omia.BLL.UnitOfWork;
using Omia.DAL.Models.Enums;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Omia.BLL.Helpers
{
    public static class CourseAccessHelper
    {
        public static async Task<bool> CheckUserPermissionAsync(IUnitOfWork unitOfWork, Guid userId, Guid courseId, AssistantPermissions requiredPermission = AssistantPermissions.None)
        {
            var course = await unitOfWork.Courses.GetByIdAsync(courseId);
            if (course?.TeacherId == userId)
                return true;

            var assistantCourse = (await unitOfWork.AssistantCourses
              .FindAsync(ac => ac.CourseId == courseId && ac.AssistantId == userId))
                  .FirstOrDefault();

            if (assistantCourse == null)
                return false;

            if (requiredPermission != AssistantPermissions.None)
            {
                return assistantCourse.Permissions.HasFlag(requiredPermission);
            }

            return true;
        }


        public static async Task<bool> CheckUserCourseAccessAsync(IUnitOfWork unitOfWork, Guid userId, Guid courseId)
        {
            var student = await unitOfWork.Students.GetByIdAsync(userId);
            if (student != null)
                return await unitOfWork.CourseStudents.IsStudentEnrolledInCourseAsync(userId, courseId);

            var isTeacher = await unitOfWork.CourseContentComments.IsTeacherInCourseAsync(userId, courseId);
            if (isTeacher) return true;

            return await unitOfWork.CourseContentComments.IsAssistantInCourseAsync(userId, courseId);
        }
    }
}
