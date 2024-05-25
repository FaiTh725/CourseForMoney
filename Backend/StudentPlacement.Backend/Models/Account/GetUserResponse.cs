namespace StudentPlacement.Backend.Models.Account
{
    public class GetUserResponse : GetAllUsersResponse
    { 
        public string? GroupName { get; set; } 

        public int? IdAllocationRequest { get; set; }   

        public string? NameAdressAllocationrequestRequest { get; set; }

        public int? CountPlace {  get; set; }
    }
}
