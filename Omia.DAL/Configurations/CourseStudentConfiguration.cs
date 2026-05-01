using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseStudentConfiguration : IEntityTypeConfiguration<CourseStudent>
    {
        public void Configure(EntityTypeBuilder<CourseStudent> builder)
        {
            builder.HasKey(cs => new { cs.CourseId, cs.StudentId });

            builder.HasOne(cs => cs.Course)
                   .WithMany(c => c.CourseStudents)
                   .HasForeignKey(cs => cs.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cs => cs.Student)
                   .WithMany(s => s.CourseStudents)
                   .HasForeignKey(cs => cs.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
