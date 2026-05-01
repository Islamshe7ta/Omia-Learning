using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class AssistantCourseConfiguration : IEntityTypeConfiguration<AssistantCourse>
    {
        public void Configure(EntityTypeBuilder<AssistantCourse> builder)
        {
            builder.HasKey(ac => new { ac.CourseId, ac.AssistantId });

            builder.HasOne(ac => ac.Course)
                   .WithMany(c => c.AssistantCourses)
                   .HasForeignKey(ac => ac.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ac => ac.Assistant)
                   .WithMany(a => a.AssistantCourses)
                   .HasForeignKey(ac => ac.AssistantId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
