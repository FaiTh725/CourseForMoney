using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Structure;
using StudentPlacement.Backend.Services.Interfaces;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class StructureService : IStructureService
    {
        private readonly IDepartmentsRepository departmentsRepository;
        private readonly ISpecialityRepository specialityRepository;
        private readonly IGroupRepository groupRepository;

        public StructureService(IDepartmentsRepository departmentsRepository,
            ISpecialityRepository specialityRepository,
            IGroupRepository groupRepository)
        {
            this.specialityRepository = specialityRepository;
            this.departmentsRepository = departmentsRepository;
            this.groupRepository = groupRepository;
        }

        public async Task<BaseResponse> CreateDepartment(CreateDepartmentRequest request)
        {
            try
            {
                var department = await departmentsRepository.GetDepartmentByName(request.Name);

                if (department != null)
                {
                    return new BaseResponse
                    {
                        Description = "Кафедру уже существует",
                        StatusCode = Domain.Enums.StatusCode.DepartmentExist
                    };
                }

                var newDepartment = new Department
                {
                    Name = request.Name,
                    HeadOfDepartment = request.HeadOfDepartment,
                    ShortName = request.ShortName,
                };

                await departmentsRepository.CreateDepartment(newDepartment);

                return new BaseResponse
                {
                    Description = "Добавили кафедру",
                    StatusCode = Domain.Enums.StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse
                {
                    StatusCode = Domain.Enums.StatusCode.ServerError,
                    Description = "Ошибка сервера"
                };
            }
        }

        public async Task<BaseResponse> CreateGroup(CreateGroupRequest request)
        {
            try
            {
                var specialization = await specialityRepository.GetSpecializationById(request.IdSpeciality);

                if(specialization == null)
                {
                    return new BaseResponse
                    {
                        StatusCode = Domain.Enums.StatusCode.NotFoundSpecialization,
                        Description = "Специальность не найдена"
                    };
                }

                var group = await groupRepository.GetGroupByName(request.Number);

                if(group != null && group.SpecializationId == specialization.Id)
                {
                    return new BaseResponse
                    {
                        StatusCode = Domain.Enums.StatusCode.GroupExist
                    };
                }

                var newGroup = new Group
                {
                    Number = request.Number,
                    Specialization = specialization,
                    SpecializationId = specialization.Id
                };

                var createdGroup = await groupRepository.CreateGroup(newGroup);

                specialization.Groups.Add(createdGroup);

                await specialityRepository.UpdateSpecialization(specialization.Id, specialization);

                return new BaseResponse 
                { 
                    Description = "Добавили группу",
                    StatusCode = Domain.Enums.StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = Domain.Enums.StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse> CreateSpeciality(CreateSpecialityRequest request)
        {
            try
            {
                var department = await departmentsRepository.GetDepartmentById(request.IdDepartment);

                if (department == null)
                {
                    return new BaseResponse
                    {
                        Description = "Кафедра не найдена",
                        StatusCode = Domain.Enums.StatusCode.NotFoundDepartment
                    };
                }

                var specialization = await specialityRepository.GetSpecializationByName(request.Name);

                if (specialization != null && specialization.DepartmentId == department.Id)
                {
                    return new BaseResponse
                    {
                        Description = "Специальность уже существует",
                        StatusCode = Domain.Enums.StatusCode.SpecializationExist
                    };
                }

                var newSpecialization = new Specialization
                {
                    Code = request.Code,
                    Name = request.Name,
                    ShortName = request.ShortName,
                    Department = department,
                    DepartmentId = department.Id
                };

                var createdSpecialization = await specialityRepository.CreateSpecialization(newSpecialization);

                department.Specializations.Add(createdSpecialization);

                await departmentsRepository.UpdateDepartment(department.Id, department);

                return new BaseResponse
                {
                    Description = "Добавили специальность",
                    StatusCode = Domain.Enums.StatusCode.Ok
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = Domain.Enums.StatusCode.ServerError
                };
            }

        }

        public async Task<DataResponse<IEnumerable<GetDepartmentsWithSpecResponse>>> GetAllCpecialities()
        {
            try
            {
                var departments = await departmentsRepository.GetAllDepartmentsWithIncludes();
                //var specialization = specialityRepository.GetAllSpecializations().ToList();

                return new DataResponse<IEnumerable<GetDepartmentsWithSpecResponse>>
                {
                    Data = departments.Select(x => new GetDepartmentsWithSpecResponse
                    {
                        Id = x.Id,
                        Name = x.Name,
                        DepartmentSpeciality = x.Specializations.Select(y => new GetAllOptionsResponse
                            {
                                Id = y.Id,
                                Name = y.Name
                            }).ToList(),
                    }),
                    Description = "Получили все кафедры и их специальности",
                    StatusCode = Domain.Enums.StatusCode.Ok
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<GetDepartmentsWithSpecResponse>>
                {
                    StatusCode = Domain.Enums.StatusCode.ServerError,
                    Description = "Ошибка сервера",
                    Data = new List<GetDepartmentsWithSpecResponse>()
                };
            }
        }

        public async Task<DataResponse<IEnumerable<GetAllOptionsResponse>>> GetAllDepartments()
        {
            try
            {
                var departments = await departmentsRepository.GetAllDepartmentsWithIncludes();

                return new DataResponse<IEnumerable<GetAllOptionsResponse>>
                {
                    Data = departments.Select(x => new GetAllOptionsResponse
                    {
                        Id = x.Id,
                        Name = x.Name
                    }),
                    Description = "Получили все кафедры",
                    StatusCode = Domain.Enums.StatusCode.Ok
                };
            }
            catch
            {
                return new DataResponse<IEnumerable<GetAllOptionsResponse>>
                {
                    StatusCode = Domain.Enums.StatusCode.ServerError,
                    Description = "Ошибка сервера",
                    Data = new List<GetAllOptionsResponse>()
                };
            }
        }
    }
}
