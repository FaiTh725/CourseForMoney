using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Allocation;
using StudentPlacement.Backend.Models.Doc;
using StudentPlacement.Backend.Models.Profile;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> CreateStudent(Student student);

        Task<Student?> FindStudentByLoginAndFullName(string loginName, string fullName);

        Task<Student> GetStudentByLogin(string login);

        Task<Student> UpdateStudentByLogin(string loginStudent, Student student);

        Task<Student> UpdateStudentById(int studentId, Student student);

        Task<Student> GetStudentById(int id);

        Task<IEnumerable<Student>> GetAllStudents();

        Task<IEnumerable<GetStudentAllocationResponse>> GetAllStudentWithRequestAndOrganization(int idGroup);

        Task<GetStudentRequestResponse> GetStudentRequest(int idUser);

        Task<List<ReportStudentAllocation>> GetStudentForReport(int idGroup);

    }
}
