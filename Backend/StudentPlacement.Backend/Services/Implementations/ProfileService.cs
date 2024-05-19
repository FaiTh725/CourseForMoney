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
                var user = await userRepository.GetUser(request.IdUser);

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
                            Adress = request.AdressAllocationRequest,
                            CountPlace = request.CountPlace
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
                        IdAllocatinRequest = newOrganization.Id
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
    }
}
