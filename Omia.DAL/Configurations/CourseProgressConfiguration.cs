using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseProgressConfiguration : IEntityTypeConfiguration<CourseProgress>
    {
        public void Configure(EntityTypeBuilder<CourseProgress> builder)
        {
            builder.HasOne(p => p.Course)
                   .WithMany(c => c.Progresses)
                   .HasForeignKey(p => p.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Student)
                   .WithMany(s => s.CourseProgresses)
                   .HasForeignKey(p => p.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Content)
                   .WithMany()
                   .HasForeignKey(p => p.ContentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
