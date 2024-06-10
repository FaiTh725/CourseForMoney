using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Profile;
using StudentPlacement.Backend.Services.Implementations;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IProfileService
    {
        Task<DataResponse<AddAllocationResponse>> AddAllocationRequest(AddAllocationRequestRequest request);

        Task<BaseResponse> DeleteAllocationRequest(DeleteAllocationRequest request);

        Task<BaseResponse> UpdateProfileOrganization(ChangeProfileRequest request);

        Task<BaseResponse> UpdateRequest(ChangeRequest request);

        Task<DataResponse<GetStudentRequestResponse>> GetStudentRequest(int idUser);

        Task<DataResponse<HomeProfileResponse>> GetUserHomeProfile(int idUser);

        Task<DataResponse<UploadFileRequestResponse>> UploadFileRequest(AddOrderFileRequest request);

        Task<DataResponse<GetUserProfileResponse>> GetUserProfile(int idUser);

        Task<DataResponse<GetOrganizationProfileResponse>> GetOrganizationProfile(int idUser);

        Task<DataResponse<GetStudentProfileResponse>> GetStudentProfile(int idUser);

        Task<DataResponse<IEnumerable<GetAllocationRequestResponse>>> GetAllRequestsOrganization(int idOrganization);

        Task<byte[]> GetOrderAllocationRequest(int allocationRequestId);
    }
}
