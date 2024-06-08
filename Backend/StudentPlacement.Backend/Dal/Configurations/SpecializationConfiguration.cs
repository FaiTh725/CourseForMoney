using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Configurations
{
    public class SpecializationConfiguration : IEntityTypeConfiguration<Specialization>
    {
        public void Configure(EntityTypeBuilder<Specialization> builder)
        {
            /*builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Group)
                .WithOne(x => x.Specialization)
                .HasForeignKey<Group>(x => x.SpecializationId);

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Specializations);*/
        }
    }
}
