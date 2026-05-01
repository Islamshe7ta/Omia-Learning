using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class QuizConfiguration : IEntityTypeConfiguration<Quiz>
    {
        public void Configure(EntityTypeBuilder<Quiz> builder)
        {
            builder.HasOne(q => q.Course)
                   .WithMany(c => c.Quizzes)
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.Category)
                   .WithMany(cat => cat.Quizzes)
                   .HasForeignKey(q => q.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(q => q.Creator)
                   .WithMany()
                   .HasForeignKey(q => q.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Questions)
                   .WithOne(qn => qn.Quiz)
                   .HasForeignKey(qn => qn.QuizId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(q => q.Attempts)
                   .WithOne(att => att.Quiz)
                   .HasForeignKey(att => att.QuizId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
