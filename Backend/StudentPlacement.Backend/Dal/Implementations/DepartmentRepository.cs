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

        public async Task<IEnumerable<Department>> GetAllDepartmentsWithIncludes()
        {
            return await context.Departments
                    .Include(x => x.Specializations)
                    .ThenInclude(y => y.Groups).ToListAsync();
        }
    }
}
