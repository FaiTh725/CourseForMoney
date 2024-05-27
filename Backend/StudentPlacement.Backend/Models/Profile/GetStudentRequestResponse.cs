namespace StudentPlacement.Backend.Models.Profile
{
    public class GetStudentRequestResponse
    {
        public int? IdRequest { get; set; } 

        public string? RequestNameOrganization { get; set; }

        public string? RequestAdressRequest { get; set; }

        public string? RequestContacts { get; set; }
    }
}
