using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Domain.Entities
{
    public class Student
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public double AverageScore {  get; set; }

        public string Adress { get; set; } = string.Empty;

        public StatusAllocationRequest StatusRequest { get; set; } = StatusAllocationRequest.NoRequest;

        public bool IsMarried { get; set; }

        public bool ExtendedFamily { get; set; }

        public int? IdAllocationRequest { get; set; } 

        public AllocationRequest? AllocationRequest { get; set; }
    
        public int UserId {  get; set; }

        public User User { get; set; }

        public int GroupId { get; set; }

        public Group Group { get; set; }
    }
}
