using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class QuizQuestionConfiguration : IEntityTypeConfiguration<QuizQuestion>
    {
        public void Configure(EntityTypeBuilder<QuizQuestion> builder)
        {
            builder.HasOne(qn => qn.Quiz)
                   .WithMany(q => q.Questions)
                   .HasForeignKey(qn => qn.QuizId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(qn => qn.QuizAnswers)
                   .WithOne(qa => qa.Question)
                   .HasForeignKey(qa => qa.QuestionId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
