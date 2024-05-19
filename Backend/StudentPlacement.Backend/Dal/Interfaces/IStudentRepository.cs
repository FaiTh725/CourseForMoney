using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IStudentRepository
    {
        Task<Student> CreateStudent(Student student);

        Task<Student?> FindStudentByLoginAndFullName(string loginName, string fullName);

        Task<Student> GetStudentByLogin(string login);

        Task<Student> UpdateStudentByLogin(string loginStudent, Student student);
    }
}
