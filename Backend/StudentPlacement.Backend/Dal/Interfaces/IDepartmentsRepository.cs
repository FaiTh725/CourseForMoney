using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IDepartmentsRepository
    {
        Task<IEnumerable<Department>> GetAllDepartmentsWithIncludes();

        Task<Department> CreateDepartment(Department department);

        Task<Department> GetDepartmentByName(string nameDepartment);

        Task<Department> GetDepartmentById(int idDepartment);

        Task<Department> UpdateDepartment(int idDepartment, Department newDepartment);
    }
}
