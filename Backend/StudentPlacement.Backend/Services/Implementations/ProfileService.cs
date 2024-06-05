using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Profile;
using StudentPlacement.Backend.Services.Interfaces;
using StudentPlacement.Backend.Domain.Enums;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Services.Implementations
{
    public class ProfileService : IProfileService
    {
        private readonly IUserRepository userRepository;
        private readonly IOrganizationRepository organizationRepository;
        private readonly IAllocationRequestRepository allocationRequestRepository;
        private readonly IStudentRepository studentRepository;

        public ProfileService(IUserRepository userRepository,
                IOrganizationRepository organizationRepository,
                IAllocationRequestRepository allocationRequestRepository,
                IStudentRepository studentRepository)
        {
            this.userRepository = userRepository;
            this.organizationRepository = organizationRepository;
            this.allocationRequestRepository = allocationRequestRepository;
            this.studentRepository = studentRepository;
        }

        public async Task<DataResponse<AddAllocationResponse>> AddAllocationRequest(AddAllocationRequestRequest request)
        {
            try
            {
                var user = await userRepository.GetUser(request.Id);

                if (user == null) 
                {
                    return new DataResponse<AddAllocationResponse>
                    {
                        Description = "Пользователь не найден",
                        StatusCode = StatusCode.NotFoundUser,
                        Data =new()
                    };
                }

                var createdAllocationRequest = await allocationRequestRepository
                        .CreateAllocationRequest(new AllocationRequest
                        {
                            Adress = request.AllocationRequestAdress,
                            CountPlace = request.CountSpace
                        });

                var organization = await organizationRepository.GetOrganizationByLogin(user.Login);

                organization.AllocationRequest = createdAllocationRequest;
                organization.AllocationRequestId = createdAllocationRequest.Id;

                var newOrganization = await organizationRepository.UpdateOrganizationById(organization.Id, organization);

                return new DataResponse<AddAllocationResponse>
                {
                    Description = "Добавили запрос на распределение",
                    StatusCode = StatusCode.Ok,
                    Data = new AddAllocationResponse
                    {
                        IdAllocatinRequest = createdAllocationRequest.Id
                    }
                };
            }
            catch 
            {
                return new DataResponse<AddAllocationResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new()
                };
            }
        }

        public async Task<BaseResponse> DeleteAllocationRequest(DeleteAllocationRequest request)
        {
            try
            {
                var allocation = await allocationRequestRepository.GetAllocationRequestById(request.IdRequest);
                var organization = await organizationRepository.FindOrganizationByLoginAndName(request.LoginUser, request.OrganizationName);

                if(allocation == null || organization == null)
                {
                    return new BaseResponse
                    {
                        Description = "Запрос или орагинизация не найден",
                        StatusCode = StatusCode.NotFountRequest
                    };
                }

                await organizationRepository.DeleteAllocationRequest(organization);

                await studentRepository.DeleteRequestInStudents(allocation);

                await allocationRequestRepository.DeleteAllocationRequest(allocation);

                //await studentRepository.DeleteRequestInStudents(allocation);

                return new BaseResponse
                {
                    Description = "Удалили заявку",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                };
            }
        }

        public async Task<DataResponse<GetStudentRequestResponse>> GetStudentRequest(int idUser)
        {
            try
            {
                var student = await studentRepository.GetStudentRequest(idUser);

                return new DataResponse<GetStudentRequestResponse>
                {
                    Data = student,
                    Description = "Получили заявку",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch
            {
                return new DataResponse<GetStudentRequestResponse>
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                    Data = new()
                };
            }
        }

        public async Task<DataResponse<HomeProfileResponse>> GetUserHomeProfile(int idUser)
        {
            try
            {
                var user = await userRepository.GetById(idUser);

                if(user == null)
                {
                    return new DataResponse<HomeProfileResponse>
                    {
                        Data = new(),
                        Description = "Пользователь не найден",
                        StatusCode= StatusCode.NotFoundUser,
                    };
                }

                return new DataResponse<HomeProfileResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Получили логин и картинку пользователя",
                    Data = new HomeProfileResponse
                    {
                        Id = user.Id,
                        Login = user.Login,
                        Image = user.ImageUserStringFormat
                    }
                };
            }
            catch
            {
                return new DataResponse<HomeProfileResponse>
                {
                    Data = new(),
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError
                };
            }
        }

        public async Task<BaseResponse> UpdateProfileOrganization(ChangeProfileRequest request)
        {
            try
            {
                var organization = await organizationRepository.GetOrganizationByLogin(request.LoginUser);

                if(organization == null) 
                {
                    return new BaseResponse 
                    { 
                        Description = "Организация не найдена",
                        StatusCode = StatusCode.Ok
                    };
                }

                if(request.AllocationId == null)
                {
                    await organizationRepository.UpdateOrganizationByLogin(request.LoginUser, new Organization
                    {
                        Name = request.OrganizationName,
                        Contacts = organization.Contacts,
                    });

                    return new BaseResponse
                    {
                        Description = "Обновили организацию",
                        StatusCode = StatusCode.Ok,
                    };
                }

                var oldAllocation = await allocationRequestRepository.GetAllocationRequestById(request.AllocationId ?? -1);

                oldAllocation.Adress = request.Adress;
                oldAllocation.CountPlace = request.CountPlace ?? -1;

                await organizationRepository.UpdateOrganizationByLogin(request.LoginUser, new Organization
                {
                    Name = request.OrganizationName,
                    Contacts = request.Contact,
                    AllocationRequest = oldAllocation,
                    AllocationRequestId = request.AllocationId
                });

                return new BaseResponse 
                { 
                    Description = "Обновили организацию",
                    StatusCode = StatusCode.Ok,
                };
            }
            catch
            {
                return new BaseResponse
                {
                    Description = "Ошибка сервера",
                    StatusCode = StatusCode.ServerError,
                };
            }
        }
    }
}
