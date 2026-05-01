using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Course
{
    public class CourseDiscussionDTO
    {
        public Guid Id { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public string SenderName { get; set; }
        public string MessageType { get; set; }
    }
}
