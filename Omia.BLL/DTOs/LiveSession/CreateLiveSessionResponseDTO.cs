using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.LiveSession
{
    public class CreateLiveSessionResponseDTO :BaseResponseDTO
    {

        public Guid LiveSessionId { get; set; }
        public DateTime CreatedAt { get; set; }
}
}
