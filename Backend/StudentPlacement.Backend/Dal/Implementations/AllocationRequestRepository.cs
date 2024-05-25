using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteAllocationRequest(AllocationRequest allocationRequest)
        {
            context.AllocationRequests.Remove(allocationRequest);

            await context.SaveChangesAsync();
        }

        public async Task<AllocationRequest> GetAllocationRequestById(int id)
        {
            return await context.AllocationRequests.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<AllocationRequest> UpdateAllocationRequest(int idAllocationRequest, AllocationRequest allocationRequest)
        {
            var oldRequest = await GetAllocationRequestById(idAllocationRequest);

            var students = new List<Student>(allocationRequest.Students);
            oldRequest.Students.Clear();
            oldRequest.Students.AddRange(students);
            oldRequest.Adress = allocationRequest.Adress;
            oldRequest.CountPlace = allocationRequest.CountPlace;

            await context.SaveChangesAsync();

            return oldRequest;
        }
    }
}
