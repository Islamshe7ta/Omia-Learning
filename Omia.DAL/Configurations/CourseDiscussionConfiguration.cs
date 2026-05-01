using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseDiscussionConfiguration : IEntityTypeConfiguration<CourseDiscussion>
    {
        public void Configure(EntityTypeBuilder<CourseDiscussion> builder)
        {
            builder.HasOne(d => d.Course)
                   .WithMany(c => c.Discussions)
                   .HasForeignKey(d => d.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Sender)
                   .WithMany()
                   .HasForeignKey(d => d.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
