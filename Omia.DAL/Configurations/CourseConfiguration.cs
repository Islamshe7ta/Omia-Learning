using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseConfiguration : IEntityTypeConfiguration<Course>
    {
        public void Configure(EntityTypeBuilder<Course> builder)
        {
            builder.HasOne(c => c.Teacher)
                   .WithMany(t => t.Courses)
                   .HasForeignKey(c => c.TeacherId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Institute)
                   .WithMany(i => i.Courses)
                   .HasForeignKey(c => c.InstituteId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.CourseStudents)
                   .WithOne(cs => cs.Course)
                   .HasForeignKey(cs => cs.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Categories)
                   .WithOne(cat => cat.Course)
                   .HasForeignKey(cat => cat.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Contents)
                   .WithOne(cnt => cnt.Course)
                   .HasForeignKey(cnt => cnt.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.LiveSessions)
                   .WithOne(ls => ls.Course)
                   .HasForeignKey(ls => ls.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Assignments)
                   .WithOne(a => a.Course)
                   .HasForeignKey(a => a.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Quizzes)
                   .WithOne(q => q.Course)
                   .HasForeignKey(q => q.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Discussions)
                   .WithOne(d => d.Course)
                   .HasForeignKey(d => d.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(c => c.Progresses)
                   .WithOne(p => p.Course)
                   .HasForeignKey(p => p.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
