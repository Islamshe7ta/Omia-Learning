using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseContentConfiguration : IEntityTypeConfiguration<CourseContent>
    {
        public void Configure(EntityTypeBuilder<CourseContent> builder)
        {
            builder.HasOne(cnt => cnt.Course)
                   .WithMany(c => c.Contents)
                   .HasForeignKey(cnt => cnt.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cnt => cnt.Category)
                   .WithMany(cat => cat.Contents)
                   .HasForeignKey(cnt => cnt.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(cnt => cnt.Uploader)
                   .WithMany()
                   .HasForeignKey(cnt => cnt.UploadedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
