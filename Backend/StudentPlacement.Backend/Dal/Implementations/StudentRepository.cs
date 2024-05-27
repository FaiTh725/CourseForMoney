﻿using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Account;
using StudentPlacement.Backend.Models.Allocation;
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
                .Include(x => x.User).Include(x => x.AllocationRequest)
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
                        IdRequest = x.AllocationRequest.Id,
                        IdOrganization = context.Organizations.Include(x => x.AllocationRequest)
                            .FirstOrDefault(y => x.AllocationRequest.Id == y.AllocationRequestId).Id,
                        NameOrganization = context.Organizations.Include(x => x.AllocationRequest)
                            .FirstOrDefault(y => x.AllocationRequest.Id == y.AllocationRequestId).Name,
                        Contacts = context.Organizations.Include(x => x.AllocationRequest)
                            .FirstOrDefault(y => x.AllocationRequest.Id == y.AllocationRequestId).Contacts

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

        public async Task<GetStudentRequestResponse> GetStudentRequest(int idUser)
        {
            /*var query = from student in context.Students
                        join request in context.AllocationRequests on student.IdAllocationRequest equals request.Id into sr
                        join user in context.Users on student.UserId equals user.Id into su
                        join orgnization in context.Organizations on sr.
*/
            var request = await context.Students.Include(x => x.User).Include(x => x.AllocationRequest)
                .FirstOrDefaultAsync(x => x.User.Id == idUser);

            if(request == null)
            {
                return new GetStudentRequestResponse
                {
                    IdRequest = null,
                    RequestAdressRequest = null,
                    RequestContacts = null,
                    RequestNameOrganization = null
                };
            }

            var data = (await context.Organizations.Include(x => x.AllocationRequest)
                .FirstOrDefaultAsync(x => x.AllocationRequestId == request.IdAllocationRequest));

            return new GetStudentRequestResponse
            {
                IdRequest = data?.AllocationRequestId,
                RequestAdressRequest = data?.AllocationRequest.Adress,
                RequestContacts = data?.Contacts,
                RequestNameOrganization = data?.Name
            };
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
