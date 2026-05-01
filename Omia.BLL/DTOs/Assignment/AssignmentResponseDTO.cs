using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Assignment
{
    public class AssignmentResponseDTO : BaseResponseDTO
    {
       public Guid AssignmentId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}


