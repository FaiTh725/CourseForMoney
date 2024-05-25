using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Models.Allocation
{
    public class GetStudentAllocationResponse
    {
        public int IdStudent {  get; set; }

        public string FullName { get; set; }

        public double AverageScore { get; set; }    

        public StatusAllocationRequest Status { get; set; }

        public AllocationRequestView? Request { get; set; } = null;
    
    }

    public class AllocationRequestView
    {
        public int? IdRequest { get; set; } 
        
        public int? IdOrganization {  get; set; }

        public string? NameOrganization { get; set; }

        public string? AdressRequest {  get; set; }

        public string? Contacts { get; set; }
    }
}
