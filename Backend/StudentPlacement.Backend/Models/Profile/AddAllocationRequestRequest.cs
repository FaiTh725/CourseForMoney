namespace StudentPlacement.Backend.Models.Profile
{
    public class AddAllocationRequestRequest
    {
        public int IdOrganization { get; set; }

        public string Adress { get; set; }
    
        public string Specialist { get; set; }

        public int CountFreePlace {  get; set; }
    }
}
