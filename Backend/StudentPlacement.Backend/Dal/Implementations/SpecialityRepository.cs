using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class SpecialityRepository : ISpecialityRepository
    {
        private readonly AppDbContext context;

        public SpecialityRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Specialization> CreateSpecialization(Specialization specialization)
        {
            var createdSpeciality = await context.Specializations.AddAsync(specialization);

            await context.SaveChangesAsync();

            return createdSpeciality.Entity;
        }

        public IEnumerable<Specialization> GetAllSpecializations()
        {
            return context.Specializations;
        }

        public async Task<Specialization> GetSpecializationById(int idSpeciality)
        {
            return await context.Specializations.Include(x => x.Department).FirstOrDefaultAsync(x => x.Id == idSpeciality);
        }

        public async Task<Specialization> GetSpecializationByName(string nameSpeciality)
        {
            return await context.Specializations.Include(x => x.Department).FirstOrDefaultAsync(x => x.Name == nameSpeciality);
        }

        public async Task<Specialization> UpdateSpecialization(int idSpecilization, Specialization specialization)
        {
            var oldSpeciality = await GetSpecializationById(idSpecilization);

            oldSpeciality.Code = specialization.Code;
            oldSpeciality.ShortName = specialization.ShortName;
            oldSpeciality.Name = specialization.Name;
            oldSpeciality.DepartmentId = specialization.DepartmentId;
            oldSpeciality.Department = specialization.Department;

            var groups = new List<Group>(specialization.Groups);

            oldSpeciality.Groups.Clear();
            oldSpeciality.Groups.AddRange(groups);


            return oldSpeciality;
        }
    }
}
