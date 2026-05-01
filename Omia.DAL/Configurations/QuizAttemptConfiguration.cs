using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class QuizAttemptConfiguration : IEntityTypeConfiguration<QuizAttempt>
    {
        public void Configure(EntityTypeBuilder<QuizAttempt> builder)
        {
            builder.HasOne(att => att.Quiz)
                   .WithMany(q => q.Attempts)
                   .HasForeignKey(att => att.QuizId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(att => att.Student)
                   .WithMany(s => s.QuizAttempts)
                   .HasForeignKey(att => att.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(att => att.Answers)
                   .WithOne(qa => qa.Attempt)
                   .HasForeignKey(qa => qa.AttemptId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
