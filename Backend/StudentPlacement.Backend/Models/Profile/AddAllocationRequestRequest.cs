namespace StudentPlacement.Backend.Models.Profile
{
    public class AddAllocationRequestRequest
    {
        public int Id { get; set; }

        public string OrganizationName { get; set; }
    
        public string AllocationRequestAdress {  get; set; }

        public int CountSpace {  get; set; }
    }
}
