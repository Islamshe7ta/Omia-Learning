using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.CourseContent
{
    public class ContentCountDTO : BaseResponseDTO
    {
        public int VideoCount { get; set; }
        public int FileCount { get; set; }
        public int LiveSessionCount { get; set; }
        public int PdfCount { get; set; }

    }
}
