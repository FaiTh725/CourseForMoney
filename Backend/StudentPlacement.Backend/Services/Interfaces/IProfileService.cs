using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Profile;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IProfileService
    {
        Task<DataResponse<AddAllocationResponse>> AddAllocationRequest(AddAllocationRequestRequest request);

        Task<BaseResponse> DeleteAllocationRequest(DeleteAllocationRequest request);

        Task<BaseResponse> UpdateProfileOrganization(ChangeProfileRequest request);
    }
}
