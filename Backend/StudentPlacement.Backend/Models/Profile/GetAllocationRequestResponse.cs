namespace StudentPlacement.Backend.Models.Profile
{
    public class GetAllocationRequestResponse
    {
        public int IdRequest { get; set; }

        public string Adress { get; set; }

        public string Specialist { get; set; }

        public int CountPlace { get; set; }

        public string UrlOrderFile { get; set; }
    }
}
