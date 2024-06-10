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
            throw new NotImplementedException();
        }

        public async Task<Organization?> FindOrganizationByLoginAndName(string login, string name)
        {
            return await context.Organizations.Include(x => x.User)
                .FirstOrDefaultAsync(x => x.User.Login == login && x.Name == name);
        }

        public async Task<IEnumerable<Organization>> GetAllOrganizations()
        {
            return await context.Organizations.Include(x => x.AllocationRequests).ThenInclude(x => x.Students).ToListAsync();
        }

        public async Task<Organization> GetOrganizationById(int idOrganization)
        {
            return await context.Organizations
                .Include(x => x.AllocationRequests)
                .Include(x => x.User)
                .FirstOrDefaultAsync(x => x.Id == idOrganization);
        }

        public async Task<Organization> GetOrganizationByIdUser(int idUser)
        {
            return await context.Organizations
                .Include(x => x.User)
                .Include(x => x.AllocationRequests).FirstOrDefaultAsync(x => x.UserId == idUser);
        }

        public async Task<Organization> GetOrganizationByLogin(string login)
        {
            return await context.Organizations
                .Include(x => x.User).Include(x => x.AllocationRequests)
                .FirstOrDefaultAsync(x => x.User.Login == login);

        }

        public async Task<Organization> GetOrganizationIdRequest(int idRequest)
        {
            return (await context.AllocationRequests.Include(x => x.Organization).FirstOrDefaultAsync(x => x.OrganizationId == idRequest)).Organization;
        }

        public async Task<Organization> UpdateOrganizationBase(Organization oldOrganization, Organization newOrganization)
        {
            oldOrganization.Contacts = newOrganization.Contacts;
            oldOrganization.Name = newOrganization.Name;

            var requests = new List<AllocationRequest>(newOrganization.AllocationRequests);

            oldOrganization.AllocationRequests.Clear();

            oldOrganization.AllocationRequests.AddRange(requests);

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

            return oldOrganization;
        }
    }
}
