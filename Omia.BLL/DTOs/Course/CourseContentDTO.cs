using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Course
{
    public class CourseContentDTO
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string ContentType { get; set; }
        public int OrderNumber { get; set; }
        public bool IsFree { get; set; }
        public Guid? CategoryId { get; set; }
    }
}
