using System;
using System.Text.Json.Serialization;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class MyQuizAttemptAnswerDTO
    {
        [JsonPropertyName("questionId")]
        public Guid QuestionId { get; set; }

        [JsonPropertyName("textAnswer")]
        public string? TextAnswer { get; set; }

        [JsonPropertyName("isCorrect")]
        public bool? IsCorrect { get; set; }

        [JsonPropertyName("marksObtained")]
        public float? MarksObtained { get; set; }
    }

}
