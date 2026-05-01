using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omia.BLL.DTOs.Quiz
{
    public class QuizResponseDTO:BaseResponseDTO
    {
        public Guid QuizId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
