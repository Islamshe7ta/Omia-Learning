using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseCategoryConfiguration : IEntityTypeConfiguration<CourseCategory>
    {
        public void Configure(EntityTypeBuilder<CourseCategory> builder)
        {
            builder.HasOne(cat => cat.Course)
                   .WithMany(c => c.Categories)
                   .HasForeignKey(cat => cat.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cat => cat.Contents)
                   .WithOne(cnt => cnt.Category)
                   .HasForeignKey(cnt => cnt.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cat => cat.LiveSessions)
                   .WithOne(ls => ls.Category)
                   .HasForeignKey(ls => ls.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cat => cat.Assignments)
                   .WithOne(a => a.Category)
                   .HasForeignKey(a => a.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(cat => cat.Quizzes)
                   .WithOne(q => q.Category)
                   .HasForeignKey(q => q.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
