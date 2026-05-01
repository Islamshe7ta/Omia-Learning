using System;
using System.Text.Json.Serialization;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class MyQuizAttemptQuestionDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("questionText")]
        public string QuestionText { get; set; } = string.Empty;

        [JsonPropertyName("questionType")]
        public string QuestionType { get; set; } = string.Empty;

        [JsonPropertyName("answers")]
        public string Answers { get; set; } = string.Empty;

        [JsonPropertyName("correctAnswer")]
        public string CorrectAnswer { get; set; } = string.Empty;

        [JsonPropertyName("marks")]
        public float? Marks { get; set; }

        [JsonPropertyName("orderNumber")]
        public int OrderNumber { get; set; }
    }
}
