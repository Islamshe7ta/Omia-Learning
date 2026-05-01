using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class CourseContentCommentConfiguration : IEntityTypeConfiguration<CourseContentComment>
    {
        public void Configure(EntityTypeBuilder<CourseContentComment> builder)
        {
            builder.HasOne(ccc => ccc.CourseContent)
                   .WithMany(cc => cc.Comments)
                   .HasForeignKey(ccc => ccc.CourseContentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ccc => ccc.Sender)
                   .WithMany()
                   .HasForeignKey(ccc => ccc.SenderId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
