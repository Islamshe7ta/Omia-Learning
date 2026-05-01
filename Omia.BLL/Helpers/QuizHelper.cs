using Omia.BLL.DTOs.Quiz;
using Omia.BLL.DTOs.QuizAttempt;
using Omia.DAL.Models.Entities;
using Omia.DAL.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Omia.BLL.Helpers
{
    public static class QuizHelper
    {
        public static (bool IsValid, string Message) ValidateAttemptStatus(QuizAttempt attempt)
        {
            if (attempt.Status == QuizAttemptStatus.Completed)
            {
                return (false, "Quiz already submitted");
            }

            var duration = attempt.Quiz.DurationMinutes ?? 0;
            var endTimeAllowed = attempt.StartTime.AddMinutes(duration).AddSeconds(30);

            if (DateTime.UtcNow > endTimeAllowed)
            {
                return (false, "Quiz time expired");
            }

            return (true, string.Empty);
        }

        public static (List<QuizAnswer> QuizAnswers, float TotalScore, float TotalPossibleMarks) ProcessAndGradeAnswers(
            QuizAttempt attempt, List<QuizAnswerDTO> studentAnswers)
        {
            var quizAnswers = new List<QuizAnswer>();
            float totalScore = 0;
            float totalPossibleMarks = 0;

            foreach (var question in attempt.Quiz.Questions)
            {
                totalPossibleMarks += question.Marks ?? 0;

                var studentAnswerDTO = studentAnswers.FirstOrDefault(a => a.QuestionId == question.Id);
                var quizAnswer = GradeQuestion(attempt.Id, question, studentAnswerDTO?.TextAnswer);

                if (quizAnswer.IsCorrect == true)
                {
                    totalScore += quizAnswer.MarksObtained ?? 0;
                }

                quizAnswers.Add(quizAnswer);
            }

            return (quizAnswers, totalScore, totalPossibleMarks);
        }

        public static QuizAnswer GradeQuestion(Guid attemptId, QuizQuestion question, string? studentAnswerText)
        {
            var quizAnswer = new QuizAnswer
            {
                AttemptId = attemptId,
                QuestionId = question.Id,
                TextAnswer = studentAnswerText?.Trim(),
                IsCorrect = false,
                MarksObtained = 0
            };

            if (!string.IsNullOrEmpty(quizAnswer.TextAnswer))
            {
                if (string.Equals(quizAnswer.TextAnswer, question.CorrectAnswer?.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    quizAnswer.IsCorrect = true;
                    quizAnswer.MarksObtained = question.Marks ?? 0;
                }
            }

            return quizAnswer;
        }
    }
}
