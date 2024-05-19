namespace StudentPlacement.Backend.Domain.Entities
{
    public class AllocationRequest
    {
        public int Id { get; set; }

        public string Adress { get; set; } = string.Empty;

        public int CountPlace { get; set; }
        

        /*public int OrganizationId { get; set; } 

        public Organization Organization { get; set; }

        public int DepartmentId { get; set; }

        public Department Department { get; set; }*/
    }
}
