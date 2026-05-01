using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.LiveSession
{
    public class UpdateLiveSessionDTO
    {

        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string? MeetingLink { get; set; }
        public string? RecordedVideoUrl { get; set; }
        public Guid? CategoryId { get; set; }
        public int? OrderNumber { get; set; }
    }
}
