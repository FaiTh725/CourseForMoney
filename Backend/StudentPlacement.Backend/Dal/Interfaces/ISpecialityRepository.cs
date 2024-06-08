using StudentPlacement.Backend.Domain.Entities;

namespace StudentPlacement.Backend.Dal.Interfaces
{
    public interface ISpecialityRepository
    {
        public IEnumerable<Specialization> GetAllSpecializations();

        public Task<Specialization> GetSpecializationById(int idSpeciality);
        
        public Task<Specialization> GetSpecializationByName(string nameSpeciality);

        public Task<Specialization> CreateSpecialization(Specialization specialization);

        public Task<Specialization> UpdateSpecialization(int idSpecilization, Specialization specialization);
    }
}
