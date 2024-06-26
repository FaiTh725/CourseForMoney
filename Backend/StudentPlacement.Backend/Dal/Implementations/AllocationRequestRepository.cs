﻿using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;
using StudentPlacement.Backend.Models.Allocation;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class AllocationRequestRepository : IAllocationRequestRepository
    {
        private readonly AppDbContext context;
        private readonly IWebHostEnvironment environment;

        public AllocationRequestRepository(AppDbContext context, IWebHostEnvironment environment)
        {
            this.context = context;
            this.environment = environment;
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

        public async Task<List<AllocationResponse>> GetAllRequestsWithOrganizationInfo()
        {
            return await context.AllocationRequests.Select(x => new AllocationResponse 
            { 
                IdRequest = x.Id,
                CountSpace = x.CountPlace,
                Specialist = x.Specialist,
                Adress = x.Adress,
                IdOrganization = context.Organizations.FirstOrDefault(y => y.AllocationRequestId == x.Id).Id,
                Contacts = context.Organizations.FirstOrDefault(y => y.AllocationRequestId == x.Id).Contacts,
                NameOrganization = context.Organizations.FirstOrDefault(y => y.AllocationRequestId == x.Id).Name,
                CountFreeSpace = x.CountPlace - x.Students.Count,
                UrlOrderFile = x.OrderFilePath
            }).ToListAsync();
        }

        public async Task<AllocationRequest> UpdateAllocationRequest(int idAllocationRequest, AllocationRequest allocationRequest)
        {
            var oldRequest = await GetAllocationRequestById(idAllocationRequest);

            var students = new List<Student>(allocationRequest.Students);
            oldRequest.Students.Clear();
            oldRequest.Students.AddRange(students);
            oldRequest.Adress = allocationRequest.Adress;
            oldRequest.CountPlace = allocationRequest.CountPlace;
            oldRequest.Specialist = allocationRequest.Specialist;
            oldRequest.OrderFilePath = allocationRequest.OrderFilePath;

            await context.SaveChangesAsync();

            return oldRequest;
        }
    }
}
