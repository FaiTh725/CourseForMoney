using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Configurations
{
    public class AllocationRequestConfiguration : IEntityTypeConfiguration<AllocationRequest>
    {
        public void Configure(EntityTypeBuilder<AllocationRequest> builder)
        {
            /*builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Organization)
                .WithOne(x => x.AllocationRequest)
                .HasForeignKey<Organization>(x => x.AllocationRequestId);

            builder.HasOne(x => x.Department)
                .WithMany(x => x.Requests);*/
        }
    }
}
