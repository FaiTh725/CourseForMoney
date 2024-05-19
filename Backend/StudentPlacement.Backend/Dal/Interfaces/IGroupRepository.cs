

using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface IGroupRepository
    {
        public Task<IEnumerable<Group>> GetGroups();

        public Task<Group> UpdateGroup(int idGroup, Group group);

        public Task<Group> GetGroupById(int idGroup);

        public Task AddStudentToGroup(Group group,Student student);
    }
}
