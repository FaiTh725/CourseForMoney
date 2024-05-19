namespace StudentPlacement.Backend.Domain.Entities
{
    public class Department
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public string HeadOfDepartment { get; set; } = string.Empty;  

        public List<Specialization> Specializations { get; set; } = new();
        
        public List<AllocationRequest> Requests { get; set; } = new();
    }
}
