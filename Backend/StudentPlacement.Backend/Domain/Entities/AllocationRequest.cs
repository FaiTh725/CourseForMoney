namespace StudentPlacement.Backend.Domain.Entities
{
    public class AllocationRequest
    {
        public int Id { get; set; }

        public string Adress { get; set; } = string.Empty;

        public string Specialist { get; set; } = string.Empty ;

        public int CountPlace { get; set; }

        public string OrderFilePath { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new List<Student>();
    }
}
