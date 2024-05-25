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

        public ProfileService(IUserRepository userRepository,
                IOrganizationRepository organizationRepository,
                IAllocationRequestRepository allocationRequestRepository)
        {
            this.userRepository = userRepository;
            this.organizationRepository = organizationRepository;
            this.allocationRequestRepository = allocationRequestRepository;
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

                await allocationRequestRepository.DeleteAllocationRequest(allocation);

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

                AllocationRequest allocationRequest = null;

                if(request.CountPlace != null)
                {
                    allocationRequest = new AllocationRequest
                    {
                        Adress = request.Adress,
                        CountPlace = request.CountPlace ?? 0
                    };
                }

                await organizationRepository.UpdateOrganizationByLogin(request.LoginUser, new Organization
                {
                    Name = request.OrganizationName,
                    Contacts = request.Contact,
                    AllocationRequest = allocationRequest,
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
