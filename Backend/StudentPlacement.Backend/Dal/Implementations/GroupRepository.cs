using Microsoft.EntityFrameworkCore;
using StudentPlacement.Backend.Dal.Interfaces;
using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Implementations
{
    public class GroupRepository : IGroupRepository
    {
        private readonly AppDbContext context;

        public GroupRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task AddStudentToGroup(Group group, Student student)
        {
            group.Students.Add(student);

            await context.SaveChangesAsync();
        }

        public async Task<Group> CreateGroup(Group group)
        {
            var newGroup = await context.AddAsync(group);

            await context.SaveChangesAsync();

            return newGroup.Entity;
        }

        public async Task<Group> GetGroupById(int idGroup)
        {
            return await context.Groups.Include(x => x.Specialization).FirstOrDefaultAsync(x => x.Id == idGroup);
        }

        public async Task<Group> GetGroupByName(string name)
        {
            return await context.Groups.Include(x => x.Specialization).FirstOrDefaultAsync(x => x.Number == name);
        }

        public async Task<IEnumerable<Group>> GetGroups()
        {
            return await context.Groups.ToListAsync();
        }

        public async Task<Group> UpdateGroup(int idGroup, Group group)
        {
            var newGroup = await GetGroupById(idGroup);

            newGroup.Students = new List<Student>(group.Students);
            newGroup.Specialization = group.Specialization;
            newGroup.SpecializationId = group.SpecializationId;
            newGroup.Number = group.Number;

            return newGroup;
        }
    }
}
