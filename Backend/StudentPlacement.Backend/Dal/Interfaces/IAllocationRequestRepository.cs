using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IAllocationRequestRepository
    {
        Task<AllocationRequest> CreateAllocationRequest(AllocationRequest allocationRequest);
    }
}
