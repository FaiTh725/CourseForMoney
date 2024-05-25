namespace StudentPlacement.Backend.Domain.Entities
{
    public class AllocationRequest
    {
        public int Id { get; set; }

        public string Adress { get; set; } = string.Empty;

        public int CountPlace { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
