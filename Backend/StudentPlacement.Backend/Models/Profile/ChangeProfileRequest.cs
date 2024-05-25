namespace StudentPlacement.Backend.Models.Profile
{
    public class ChangeProfileRequest
    {
        public string LoginUser { get; set; }

        public string OrganizationName { get; set; }

        public string Contact { get; set; }

        public int? AllocationId { get; set; }
        
        public string? Adress { get; set; }

        public int? CountPlace { get; set; }
    }
}
