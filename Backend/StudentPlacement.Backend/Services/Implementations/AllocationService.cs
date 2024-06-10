using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Allocation;
using StudentPlacement.Backend.Services.Interfaces;
using StudentPlacement.Backend.Domain.Enums;
using StudentPlacement.Backend.Dal.Interfaces;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class AllocationService : IAllocationService
    {
        private readonly IDepartmentsRepository departmentsRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IStudentRepository studentRepository;
        private readonly IAllocationRequestRepository allocationRequestRepository;

        public AllocationService(IDepartmentsRepository departmentsRepository,
                IOrganizationRepository organizationRepository,
                IStudentRepository studentRepository,
                IAllocationRequestRepository allocationRequestRepository)
        {
            this.departmentsRepository = departmentsRepository;
            this.organizationRepository = organizationRepository;
            this.studentRepository = studentRepository;
            this.allocationRequestRepository = allocationRequestRepository;
        }

        public async Task<DataResponse<AddRequestToUserResponse>> AddRequestToStudent(AddDeleteRequestToUserRequest request)
        {
            try
            {
                var student = await studentRepository.GetStudentById(request.IdStudent);
                var allocationRequest = await allocationRequestRepository.GetAllocationRequestById(request.IdRequest);

                if(student == null || allocationRequest == null)
                {
                    return new DataResponse<AddRequestToUserResponse>
                    {
                        Data = new AddRequestToUserResponse(),
                        Description = "Не найдена заявка или студент",
                        StatusCode = StatusCode.NotFountRequest | StatusCode.NotFountRequest
                    };
                }
                if(allocationRequest.Students.Count - 1 == allocationRequest.CountPlace)
                {
                    return new DataResponse<AddRequestToUserResponse>
                    {
                        Data = new AddRequestToUserResponse(),
                        Description = "Места в заявке закончились",
                        StatusCode = StatusCode.LimitStudent
                    };
                }

                student.AllocationRequest = allocationRequest;
                student.IdAllocationRequest = allocationRequest.Id;
                student.StatusRequest = StatusAllocationRequest.Request;

                await studentRepository.UpdateStudentById(student.Id, student);

                allocationRequest.Students.Add(student);

                await allocationRequestRepository.UpdateAllocationRequest(allocationRequest.Id, allocationRequest);

                var organizationOfRequest = await organizationRepository.GetOrganizationIdRequest(allocationRequest.OrganizationId);

                return new DataResponse<AddRequestToUserResponse>
                {
                    Data = new AddRequestToUserResponse()
                    {
                        IdStudent = student.Id,
                        StatusRequest = student.StatusRequest,
                        AllocationRequest = new AllocationRequestView
                        {
                            IdRequest = allocationRequest.Id,
                            AdressRequest = allocationRequest.Adress,
                            Contacts = organizationOfRequest.Contacts,
                            IdOrganization = organizationOfRequest.Id,
                            NameOrganization = organizationOfRequest.Name,
                            Specialist = allocationRequest.Specialist,
                            UrlOrderFile = allocationRequest.OrderFilePath
                        }

                    },
                    Description = "Добавили заявку",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new DataResponse<AddRequestToUserResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError, 
                    Data = new AddRequestToUserResponse()
                };
            }
        }

        public async Task<BaseResponse> DeleteRequestStudent(AddDeleteRequestToUserRequest request)
        {
            try
            {
                var student = await studentRepository.GetStudentById(request.IdStudent);
                var allocationRequest = await allocationRequestRepository.GetAllocationRequestById(request.IdRequest);

                if (student == null || allocationRequest == null)
                {
                    return new DataResponse<AddRequestToUserResponse>
                    {
                        Data = new AddRequestToUserResponse(),
                        Description = "Не найдена заявка или студент",
                        StatusCode = StatusCode.NotFountRequest | StatusCode.NotFountRequest
                    };
                }

                student.AllocationRequest = null;
                student.IdAllocationRequest = null;
                student.StatusRequest = StatusAllocationRequest.NoRequest;

                allocationRequest.Students.Remove(student);

                await studentRepository.UpdateStudentById(student.Id, student);
                await allocationRequestRepository.UpdateAllocationRequest(allocationRequest.Id, allocationRequest);

                return new BaseResponse
                {
                    Description = "Удили заявку от студента",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<DataResponse<IEnumerable<AllocationResponse>>> GetAllAllocations()
        {
            try
            {
                var data  = await allocationRequestRepository.GetAllRequestsWithOrganizationInfo();

                return new DataResponse<IEnumerable<AllocationResponse>>
                {
                    Description = "Получили все заявки",
                    StatusCode = StatusCode.Ok,
                    Data = data
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<AllocationResponse>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<AllocationResponse>()
                };
            }
        }

        public async Task<DataResponse<IEnumerable<AllDepartmentsAndGroupsResponse>>> GetAllDepartmentsAndGroups()
        {
            try
            {
                var departments = await departmentsRepository.GetAllDepartmentsWithIncludes();

                return new DataResponse<IEnumerable<AllDepartmentsAndGroupsResponse>>
                {
                    Description = "Получили списки кафедр",
                    StatusCode = StatusCode.Ok,
                    Data = departments.Select(x => new AllDepartmentsAndGroupsResponse
                    {
                        NameDepartment = x.Name,
                        IdDepartment = x.Id,
                        Specializations = x.Specializations.Select(y => new SpecializationView
                            {
                                IdSpecialization = y.Id,
                                ShortName = y.ShortName,
                                Groups = y.Groups.Select(z => new GroupsView
                                    {
                                        IdGroup = z.Id,
                                        NameGroup = z.Number
                                    }).ToList()
                            }).ToList()
                        }).ToList()
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<AllDepartmentsAndGroupsResponse>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<AllDepartmentsAndGroupsResponse>()
                };
            }
        }

        public async Task<DataResponse<IEnumerable<GetStudentsFromRequestResponse>>> GetSttudentsByRequest(int idRequest)
        {
            try
            {
                var students = await studentRepository.GetStudentFromRequest(idRequest);

                return new DataResponse<IEnumerable<GetStudentsFromRequestResponse>>
                {
                    Data = students,
                    Description = "Получили пользователей заявки",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch 
            {
                return new DataResponse<IEnumerable<GetStudentsFromRequestResponse>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<GetStudentsFromRequestResponse>()
                };
            }
        }

        public async Task<DataResponse<IEnumerable<GetStudentAllocationResponse>>> GetStudents(int idGroup)
        {
            try
            {
                var students = await studentRepository.GetAllStudentWithRequestAndOrganization(idGroup);

                return new DataResponse<IEnumerable<GetStudentAllocationResponse>>
                {
                    Data = students,
                    Description = "Получили пользователей",
                    StatusCode = StatusCode.Ok
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<GetStudentAllocationResponse>>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new List<GetStudentAllocationResponse>()
                };
            }
        }
    }
}
