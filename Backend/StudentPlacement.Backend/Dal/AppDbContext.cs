using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Configurations;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        public DbSet<AllocationRequest> AllocationRequests { get; set; }

        public DbSet<Department> Departments { get; set; }  

        public DbSet<Group> Groups { get; set; }
        
        public DbSet<Organization> Organizations { get; set; }  

        public DbSet<Specialization> Specializations { get; set; }  

        public DbSet<Student> Students { get; set; }    

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*modelBuilder.ApplyConfiguration(new AllocationRequestConfiguration());
            modelBuilder.ApplyConfiguration(new OrganizationConfiguration());
            modelBuilder.ApplyConfiguration(new GroupConfiguration());
            modelBuilder.ApplyConfiguration(new SpecializationConfiguration());*/

            base.OnModelCreating(modelBuilder);
        }
    }
}
