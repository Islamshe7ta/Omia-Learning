namespace Omia.BLL.DTOs.Quiz
{
    using Omia.BLL.DTOs.QuizAttempt;

    public class EndQuizResponseDTO : BaseResponseDTO
    {
        public QuizResultDTO? Result { get; set; }
    }
}
