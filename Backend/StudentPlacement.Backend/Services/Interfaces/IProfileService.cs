﻿using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Models.Profile;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IProfileService
    {
        Task<DataResponse<AddAllocationResponse>> AddAllocationRequest(AddAllocationRequestRequest request);

        Task<BaseResponse> DeleteAllocationRequest(DeleteAllocationRequest request);

        Task<BaseResponse> UpdateProfileOrganization(ChangeProfileRequest request);

        Task<DataResponse<GetStudentRequestResponse>> GetStudentRequest(int idUser);

        Task<DataResponse<HomeProfileResponse>> GetUserHomeProfile(int idUser);

        Task<BaseResponse> UploadFileRequest(AddOrderFileRequest request);

        Task<byte[]> GetOrderAllocationRequest(int allocationRequestId);
    }
}
