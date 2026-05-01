using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class QuizAnswerConfiguration : IEntityTypeConfiguration<QuizAnswer>
    {
        public void Configure(EntityTypeBuilder<QuizAnswer> builder)
        {
            builder.HasOne(qa => qa.Attempt)
                   .WithMany(att => att.Answers)
                   .HasForeignKey(qa => qa.AttemptId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(qa => qa.Question)
                   .WithMany(qn => qn.QuizAnswers)
                   .HasForeignKey(qa => qa.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
