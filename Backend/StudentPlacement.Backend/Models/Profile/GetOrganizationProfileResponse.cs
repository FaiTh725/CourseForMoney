using StudentPlacement.Backend.Models.Allocation;

namespace StudentPlacement.Backend.Models.Profile
{
    public class GetOrganizationProfileResponse
    {
        public int IdOrganization {  get; set; }

        public string Contacts { get; set; }

        public string NameOrganization { get; set; }

        public List<RequestProfileView> Requests { get; set; } = new List<RequestProfileView>();
    }

    public class RequestProfileView
    {
        public int IdRequest { get; set; }

        public string NameRequest { get; set; }

        public string Specialist { get; set; }

        public string UrlOrderFile { get; set; }

        public int CountPlace { get; set; }
    }
}
