namespace StudentPlacement.Backend.Models.Allocation
{
    public class AllocationResponse
    {
        public int IdOrganization { get; set; }

        public int IdRequest { get; set; }

        public string NameOrganization { get; set; }

        public string Contacts { get; set; }

        public string Adress {  get; set; }

        public string Specialist { get; set; }

        public string UrlOrderFile { get; set; }

        public int? CountSpace { get; set; }

        public int? CountFreeSpace { get; set; }
    }
}
