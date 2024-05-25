using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IDepartmentsRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsWithIncludes();
    }
}
