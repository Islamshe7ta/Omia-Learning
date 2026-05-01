using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> builder)
        {
            builder.HasOne(s => s.Parent)
                   .WithMany(p => p.Students)
                   .HasForeignKey(s => s.ParentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(s => s.Institute)
                   .WithMany(i => i.RegisteredStudents)
                   .HasForeignKey(s => s.InstituteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
