namespace Omia.BLL.DTOs.Quiz
{
    public class QuizDetailsResponseDTO
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;
        public QuizDetailsDTO? Data { get; set; }
    }
}
