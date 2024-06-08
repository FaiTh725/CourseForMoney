using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using System.Collections;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class DepartmentRepository : IDepartmentsRepository
    {
        private readonly AppDbContext context;

        public DepartmentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Department> CreateDepartment(Department department)
        {
            var newDepartment =  await context.Departments.AddAsync(department);

            await context.SaveChangesAsync();
            
            return newDepartment.Entity;
        }

        public async Task<IEnumerable<Department>> GetAllDepartmentsWithIncludes()
        {
            return await context.Departments
                    .Include(x => x.Specializations)
                    .ThenInclude(y => y.Groups).ToListAsync();
        }

        public async Task<Department> GetDepartmentById(int idDepartment)
        {
            return await context.Departments.Include(x => x.Specializations).FirstOrDefaultAsync(x => x.Id == idDepartment);
        }

        public async Task<Department> GetDepartmentByName(string nameDepartment)
        {
            return await context.Departments.FirstOrDefaultAsync(x => x.Name == nameDepartment);
        }

        public async Task<Department> UpdateDepartment(int idDepartment, Department newDepartment)
        {
            var oldDepartment = await GetDepartmentById(idDepartment);

            oldDepartment.HeadOfDepartment = newDepartment.HeadOfDepartment;
            oldDepartment.ShortName = newDepartment.ShortName;
            oldDepartment.Name = newDepartment.Name;

            var specialization = new List<Specialization>(newDepartment.Specializations);
            oldDepartment.Specializations.Clear();
            oldDepartment.Specializations.AddRange(specialization);

            await context.SaveChangesAsync();

            return oldDepartment;
        }
    }
}
