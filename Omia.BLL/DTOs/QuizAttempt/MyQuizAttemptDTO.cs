using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Omia.BLL.DTOs.QuizAttempt
{
    public class MyQuizAttemptDTO
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("startTime")]
        public DateTime StartTime { get; set; }

        [JsonPropertyName("endTime")]
        public DateTime? EndTime { get; set; }

        [JsonPropertyName("score")]
        public float? Score { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("attemptAt")]
        public DateTime AttemptAt { get; set; }

        [JsonPropertyName("quizQuestions ")]
        public List<MyQuizAttemptQuestionDTO> QuizQuestions { get; set; } = new List<MyQuizAttemptQuestionDTO>();

        [JsonPropertyName("quizAnswer ")]
        public List<MyQuizAttemptAnswerDTO> QuizAnswers { get; set; } = new List<MyQuizAttemptAnswerDTO>();
    }
}
