using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Configurations
{
    public class GroupConfiguration : IEntityTypeConfiguration<Group>
    {
        public void Configure(EntityTypeBuilder<Group> builder)
        {
            /*builder.HasKey(x => x.Id);

            builder.HasOne(x => x.Specialization)
                .WithOne(x => x.Group)
                .HasForeignKey<Specialization>(x => x.GroupId);

            builder.HasMany(x => x.Students)
                .WithOne(x => x.Group);*/
        }
    }
}
