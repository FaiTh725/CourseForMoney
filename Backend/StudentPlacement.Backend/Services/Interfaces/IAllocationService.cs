using StudentPlacement.Backend.Domain.Response;
using StudentPlacement.Backend.Migrations;
using StudentPlacement.Backend.Models.Allocation;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IAllocationService
    {
        public Task<DataResponse<IEnumerable<AllDepartmentsAndGroupsResponse>>> GetAllDepartmentsAndGroups();

        public Task<DataResponse<IEnumerable<AllocationResponse>>> GetAllAllocations();

        public Task<DataResponse<IEnumerable<GetStudentAllocationResponse>>> GetStudents(int idGroup);

        public Task<DataResponse<AddRequestToUserResponse>> AddRequestToStudent(AddDeleteRequestToUserRequest request);

        public Task<BaseResponse> DeleteRequestStudent(AddDeleteRequestToUserRequest request);

        public Task<DataResponse<IEnumerable<GetStudentsFromRequestResponse>>> GetSttudentsByRequest(int idRequest);
    }
}
