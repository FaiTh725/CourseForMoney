namespace StudentPlacement.Backend.Models.Account
{
    public class GetAllUsersResponse
    {
        public int Id { get; set; } 

        public string Login { get; set; }

        public string Password { get; set; }

        public int Role { get; set; }

        public string? Image { get; set; }
        /*public IFormFile? Image { get; set; }*/

        public int? Group { get; set; }

        public string? FullName { get; set; }

        public double? AverageScore { get; set; }

        public string? AdressStudent { get; set; }

        public bool? IsMarried { get; set; }

        public bool? ExtendedFamily { get; set; }


        //public int? IdOrganization { get; set; }    

        public string? NameOrganization { get; set; }

        public string? Contacts { get; set; }
    }
}
