using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Account;
using StudentPlacement.Backend.Models.Allocation;
using StudentPlacement.Backend.Models.Doc;
using StudentPlacement.Backend.Models.Profile;

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

        public async Task DeleteRequestInStudents(AllocationRequest allocation)
        {
            var students = await context.Students
        .Where(x => x.IdAllocationRequest == allocation.Id)
        .ToListAsync();

            foreach (var student in students)
            {
                student.AllocationRequest = null;
                student.IdAllocationRequest = null;
            }

            await context.SaveChangesAsync();
        }

        public async Task<Student?> FindStudentByLoginAndFullName(string loginName, string fullName)
        {
            return await context.Students.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Login == loginName && x.FullName == fullName);
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            return await context.Students.Include(x => x.AllocationRequest)
                .Include(x => x.User).ToListAsync();
        }

        public async Task<IEnumerable<GetStudentAllocationResponse>> GetAllStudentWithRequestAndOrganization(int idGroup)
        {
            return await context.Students
                .Include(x => x.User)
                .Include(x => x.AllocationRequest).ThenInclude(x => x.Organization)
                .Where(x => x.Group.Id == idGroup)
                .Select(x => new GetStudentAllocationResponse()
                {
                    IdStudent = x.Id,
                    AverageScore = x.AverageScore,
                    FullName = x.FullName,
                    Status = x.StatusRequest,
                    Request = new AllocationRequestView
                    {
                        AdressRequest = x.AllocationRequest.Adress,
                        Specialist = x.AllocationRequest.Specialist,
                        UrlOrderFile = x.AllocationRequest.OrderFilePath,
                        IdRequest = x.AllocationRequest.Id,
                        IdOrganization = x.AllocationRequest.Organization.Id,
                        NameOrganization = x.AllocationRequest.Organization.Name,
                        Contacts = x.AllocationRequest.Organization.Contacts,

                    }
                }).ToListAsync();
        }

        public async Task<Student> GetStudentById(int id)
        {
            return await context.Students.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Student> GetStudentByLogin(string login)
        {
            return await context.Students.Include(x => x.User).FirstOrDefaultAsync(x => x.User.Login == login);
        }

        public async Task<Student> GetStudentByUserId(int idUser)
        {
            return await context.Students.Include(x => x.User).Include(x => x.Group).FirstOrDefaultAsync(x => x.UserId == idUser);
        }

        public async Task<List<ReportStudentAllocation>> GetStudentForReport(int idGroup)
        {
            return await context.Students
                .Include(x => x.AllocationRequest).ThenInclude(x => x.Organization)
                .Include(x => x.Group)
                .Where(x => x.GroupId == idGroup)
                .Select(x => new ReportStudentAllocation
                {
                    AverageScore = x.AverageScore,
                    FullName = x.FullName,
                    AllocationData = new AllocationData
                    {
                        AdressRequest = x.AllocationRequest.Adress,
                        NameOrganixation = x.AllocationRequest.Organization.Name,
                        Contacts = x.AllocationRequest.Organization.Contacts
                    }
                }).ToListAsync();
        }

        public async Task<IEnumerable<GetStudentsFromRequestResponse>> GetStudentFromRequest(int idRequest)
        {
            return await context.Students
                .Where(x => x.IdAllocationRequest == idRequest)
                .Select(x => new GetStudentsFromRequestResponse
                {
                    AverageScore = x.AverageScore,
                    FullName = x.FullName,
                    Id = x.Id,
                    Adress = x.Adress,
                    ExtendedFamily = x.ExtendedFamily,
                    IsMarried = x.IsMarried
                }).ToListAsync();
        }

        public async Task<GetStudentRequestResponse> GetStudentRequest(int idUser)
        {
            var request = await context.Students.Include(x => x.User).Include(x => x.AllocationRequest)
                .FirstOrDefaultAsync(x => x.User.Id == idUser);

            if (request == null)
            {
                return new GetStudentRequestResponse
                {
                    IdRequest = null,
                    RequestAdressRequest = null,
                    RequestContacts = null,
                    RequestNameOrganization = null
                };
            }

            var data = await context.AllocationRequests
                .Include(x => x.Organization)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            return new GetStudentRequestResponse
            {
                IdRequest = data.Id,
                RequestAdressRequest = data.Adress,
                RequestContacts = data.Organization.Contacts,
                RequestNameOrganization = data.Organization.Name
            };
            throw new NotImplementedException();
        }

        public async Task<Student> UpdateStudentById(int studentId, Student student)
        {
            var oldStudent = await GetStudentById(studentId);

            oldStudent.FullName = student.FullName;
            oldStudent.StatusRequest = student.StatusRequest;
            oldStudent.IsMarried = student.IsMarried;
            oldStudent.ExtendedFamily = student.ExtendedFamily;
            oldStudent.Adress = student.Adress;
            oldStudent.User = student.User;
            oldStudent.UserId = student.UserId;
            oldStudent.AllocationRequest = student.AllocationRequest;
            oldStudent.IdAllocationRequest = student.IdAllocationRequest;
            oldStudent.AverageScore = student.AverageScore;
            oldStudent.Group = student.Group;
            oldStudent.GroupId = student.GroupId;

            await context.SaveChangesAsync();

            return oldStudent;
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
