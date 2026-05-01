using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CentralAdmin
{
    public class DetailFilterDto
    {
        public Guid? InstituteId { get; set; }
        public Guid? TeacherId { get; set; }
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public Guid? CourseId { get; set; }
    }
}
