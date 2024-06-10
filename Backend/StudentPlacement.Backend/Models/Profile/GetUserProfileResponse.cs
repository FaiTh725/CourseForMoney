namespace StudentPlacement.Backend.Models.Profile
{
    public class GetUserProfileResponse
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public int Role { get; set; }

        public string? Image { get; set; }

        public string Email { get; set; }
    }
}
