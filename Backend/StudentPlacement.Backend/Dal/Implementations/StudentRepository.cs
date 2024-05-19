using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class StudentRepository : IStudentRepository
    {
        private readonly AppDbContext context;

        public StudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Student> CreateStudent(Student student)
        {
            await context.Students.AddAsync(student);

            await context.SaveChangesAsync();

            return student;
        }

        public async Task<Student?> FindStudentByLoginAndFullName(string loginName, string fullName)
        {
            return await context.Students.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Login == loginName && x.FullName == fullName);
        }

        public async Task<Student> GetStudentByLogin(string login)
        {
            return await context.Students.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Login == login);
        }

        public async Task<Student> UpdateStudentByLogin(string loginStudent, Student student)
        {
            var changingStudent = await GetStudentByLogin(loginStudent);

            changingStudent.FullName = student.FullName;
            changingStudent.Adress = student.Adress;
            changingStudent.IsMarried = student.IsMarried;
            changingStudent.ExtendedFamily = student.ExtendedFamily;
            changingStudent.AverageScore = student.AverageScore;
            changingStudent.Group = student.Group;
            changingStudent.GroupId = student.GroupId;
            if(student.AllocationRequest != null)
            {
                changingStudent.AllocationRequest = new();
                changingStudent.AllocationRequest = student.AllocationRequest;
            }

            changingStudent.StatusRequest = student.StatusRequest;
            
            await context.SaveChangesAsync();

            return changingStudent;
        }
    }
}
