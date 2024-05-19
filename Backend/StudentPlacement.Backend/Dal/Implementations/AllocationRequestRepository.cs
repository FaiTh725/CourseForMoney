using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class AllocationRequestRepository : IAllocationRequestRepository
    {
        private readonly AppDbContext context;

        public AllocationRequestRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<AllocationRequest> CreateAllocationRequest(AllocationRequest allocationRequest)
        {
            var createAllocationRequest = await context.AllocationRequests.AddAsync(allocationRequest);

            await this.context.SaveChangesAsync();

            return createAllocationRequest.Entity;
        }
    }
}
