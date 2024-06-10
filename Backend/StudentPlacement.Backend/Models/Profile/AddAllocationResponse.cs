namespace StudentPlacement.Backend.Models.Profile
{
    public class AddAllocationResponse
    {
        public int IdRequest { get; set; }

        public string NameRequest { get; set; }

        public string Specialist {  get; set; }

        public int CountPlace {  get; set; }

        public string UrlOrderFile { get; set; }    
    }
}
