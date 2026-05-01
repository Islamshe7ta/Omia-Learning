using System.Collections.Generic;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class GetQuizAttemptsForTeacherResponseDTO : BaseResponseDTO
    {
        public IEnumerable<QuizAttemptTeacherDTO> Attempts { get; set; } = new List<QuizAttemptTeacherDTO>();
    }
}
