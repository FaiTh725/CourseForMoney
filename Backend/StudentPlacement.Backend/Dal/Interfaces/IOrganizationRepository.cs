using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IOrganizationRepository
    {
        Task<Organization> CreateOrganization(Organization organization);

        Task<Organization?> FindOrganizationByLoginAndName(string login, string name);

        Task<Organization> GetOrganizationByLogin(string login);

        Task<Organization> GetOrganizationByIdUser(int idUser);

        Task<Organization> GetOrganizationById(int idOrganization);

        Task<Organization> GetOrganizationIdRequest(int idRequest);

        Task<Organization> UpdateOrganizationByLogin(string loginOrganization, Organization organization);

        Task<Organization> UpdateOrganizationById(int idOrganization, Organization organization);
    
        Task<Organization> UpdateOrganizationBase(Organization oldOrganization, Organization newOrganization);

        Task DeleteAllocationRequest(Organization organization);

        Task<IEnumerable<Organization>> GetAllOrganizations();
    }
}
