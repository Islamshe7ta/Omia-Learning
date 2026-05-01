using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Omia.DAL.Models.Entities;

namespace Omia.DAL.Configurations
{
    public class InstituteConfiguration : IEntityTypeConfiguration<Institute>
    {
        public void Configure(EntityTypeBuilder<Institute> builder)
        {
            builder.HasMany(i => i.Teachers)
                   .WithOne(t => t.Institute)
                   .HasForeignKey(t => t.InstituteId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
