namespace Omia.BLL.DTOs.Quiz
{
    using Omia.BLL.DTOs.QuizAttempt;

    public class StartQuizResponseDTO:BaseResponseDTO
    {
       
        public QuizAttemptStartDTO? Attempt { get; set; }
    }
}
