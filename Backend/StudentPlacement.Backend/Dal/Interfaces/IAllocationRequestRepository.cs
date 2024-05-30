using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Allocation;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IAllocationRequestRepository
    {
        Task<AllocationRequest> CreateAllocationRequest(AllocationRequest allocationRequest);

        Task DeleteAllocationRequest(AllocationRequest allocationRequest);

        Task<AllocationRequest> GetAllocationRequestById(int id);

        Task<AllocationRequest> UpdateAllocationRequest(int idAllocationRequest, AllocationRequest allocationRequest);

        Task<List<AllocationResponse>> GetAllRequestsWithOrganizationInfo();
    }
}
