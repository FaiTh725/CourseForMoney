using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Configurations
{
    public class OrganizationConfiguration : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            /*builder.HasKey(x => x.Id);

            builder.HasOne(x => x.AllocationRequest)
                .WithOne(x => x.Organization)
                .HasForeignKey<AllocationRequest>(x => x.OrganizationId)
                .OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
