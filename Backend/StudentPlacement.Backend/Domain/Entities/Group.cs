namespace StudentPlacement.Backend.Domain.Entities
{
    public class Group
    {
        public int Id { get; set; }

        public string Number { get; set; }

        public int SpecializationId { get; set; }   

        public Specialization Specialization { get; set; }

        public List<Student> Students { get; set; } = new();
    }
}
