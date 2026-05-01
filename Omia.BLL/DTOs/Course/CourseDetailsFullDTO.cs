using System;
using System.Collections.Generic;

namespace Omia.BLL.DTOs.Course
{
    public class CourseDetailsFullDTO
    {
        public StudentCourseDTO CourseDetails { get; set; }
        public IEnumerable<CourseCategoryDTO> Categories { get; set; }
        public IEnumerable<CourseContentDTO> Contents { get; set; }
        public IEnumerable<LiveSessionDTO> LiveSessions { get; set; }
        public IEnumerable<AssignmentDTO> Assignments { get; set; }
        public IEnumerable<QuizDTO> Quizzes { get; set; }
        public IEnumerable<CourseDiscussionDTO> Discussions { get; set; }
    }
}
