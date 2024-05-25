using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IAllocationRequestRepository
    {
        Task<AllocationRequest> CreateAllocationRequest(AllocationRequest allocationRequest);

        Task DeleteAllocationRequest(AllocationRequest allocationRequest);

        Task<AllocationRequest> GetAllocationRequestById(int id);

        Task<AllocationRequest> UpdateAllocationRequest(int idAllocationRequest, AllocationRequest allocationRequest);
    }
}
