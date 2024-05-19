namespace StudentPlacement.Backend.Models.Profile
{
    public class AddAllocationRequestRequest
    {
        public int IdUser { get; set; }

        public string NameOrganization { get; set; }
    
        public string AdressAllocationRequest {  get; set; }

        public int CountPlace {  get; set; }
    }
}
