using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class LiveSessionConfiguration : IEntityTypeConfiguration<LiveSession>
    {
        public void Configure(EntityTypeBuilder<LiveSession> builder)
        {
            builder.HasOne(ls => ls.Course)
                   .WithMany(c => c.LiveSessions)
                   .HasForeignKey(ls => ls.CourseId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ls => ls.Category)
                   .WithMany(cat => cat.LiveSessions)
                   .HasForeignKey(ls => ls.CategoryId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(ls => ls.Creator)
                   .WithMany()
                   .HasForeignKey(ls => ls.CreatedBy)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
