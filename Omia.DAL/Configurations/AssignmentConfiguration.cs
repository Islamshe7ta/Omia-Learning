using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class AssignmentConfiguration : IEntityTypeConfiguration<Assignment>
    {
        public void Configure(EntityTypeBuilder<Assignment> builder)
        {
            builder.HasOne(a => a.Course)
                   .WithMany(c => c.Assignments)
                   .HasForeignKey(a => a.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Category)
                   .WithMany(cat => cat.Assignments)
                   .HasForeignKey(a => a.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Creator)
                   .WithMany()
                   .HasForeignKey(a => a.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(a => a.Submissions)
                   .WithOne(s => s.Assignment)
                   .HasForeignKey(s => s.AssignmentId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
