namespace StudentPlacement.Backend.Models.Account
{
    public class CreateAccountRequest
    {
        public string Password { get; set; }

        public string Login {  get; set; }

        public int Role {  get; set; }

        public int Group {  get; set; }

        public string FullName { get; set; }    

        public double AverageScore { get; set; }

        public string AdressStudent { get; set; }

        public bool IsMarried { get; set; }

        public bool ExtendedFamily { get; set; }


        public string NameOrganization { get; set; }    

        public string Contacts { get; set; }

        /*public string AdressAllocationRequest { get; set; }

        public int CountPlace { get; set; }*/
    }
}
