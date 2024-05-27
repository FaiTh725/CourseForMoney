using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class OrganizationRepository : IOrganizationRepository
    {
        private readonly AppDbContext context;

        public OrganizationRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Organization> CreateOrganization(Organization organization)
        {
            var createdOrganization =  await context.Organizations.AddAsync(organization);

            await context.SaveChangesAsync();
            
            return createdOrganization.Entity;
        }

        public async Task DeleteAllocationRequest(Organization organization)
        {
            organization.AllocationRequest = null;
            organization.AllocationRequestId = null;

            await context.SaveChangesAsync();
        }

        public async Task<Organization?> FindOrganizationByLoginAndName(string login, string name)
        {
            return await context.Organizations.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Login == login && x.Name == name);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizations()
        {
            return await context.Organizations.Include(x => x.AllocationRequest).ThenInclude(x => x.Students).ToListAsync();
        }

        public async Task<Organization> GetOrganizationById(int idOrganization)
        {
            return await context.Organizations
                .Include(x => x.AllocationRequest).Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == idOrganization);
        }

        public async Task<Organization> GetOrganizationByLogin(string login)
        {
            return await context.Organizations
                .Include(x => x.User).Include(x => x.AllocationRequest)
                .FirstOrDefaultAsync(x => x.User.Login == login);
        }

        public async Task<Organization> GetOrganizationIdRequest(int idRequest)
        {
            return await context.Organizations.Include(x => x.AllocationRequest).FirstOrDefaultAsync(x => x.AllocationRequestId == idRequest);
        }

        public async Task<Organization> UpdateOrganizationBase(Organization oldOrganization, Organization newOrganization)
        {
            oldOrganization.Contacts = newOrganization.Contacts;
            oldOrganization.Name = newOrganization.Name;
            
            if(newOrganization.AllocationRequest != null)
            {
                oldOrganization.AllocationRequest = newOrganization.AllocationRequest;
                oldOrganization.AllocationRequestId = newOrganization.AllocationRequestId;
            }

            await context.SaveChangesAsync();

            return oldOrganization;
        }

        public async Task<Organization> UpdateOrganizationById(int idOrganization, Organization organization)
        {
            var oldOrganization = await GetOrganizationById(idOrganization);

            await UpdateOrganizationBase(oldOrganization, organization);

            return oldOrganization;
        }

        public async Task<Organization> UpdateOrganizationByLogin(string loginOrganization, Organization organization)
        {
            var oldOrganization = await GetOrganizationByLogin(loginOrganization);

            await UpdateOrganizationBase(oldOrganization, organization);

            /*oldOrganization.Contacts = organization.Contacts;
            oldOrganization.Name = organization.Name;
            
            if(organization.AllocationRequest != null)
            {
                oldOrganization.AllocationRequest = new();
                oldOrganization.AllocationRequest = organization.AllocationRequest;
                oldOrganization.AllocationRequestId = organization.AllocationRequestId;
            }

            await context.SaveChangesAsync();*/

            return oldOrganization;
        }
    }
}
