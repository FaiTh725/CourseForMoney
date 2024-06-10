namespace StudentPlacement.Backend.Models.Profile
{
    public class GetStudentProfileResponse
    {
        public int? Group { get; set; }

        public string? FullName { get; set; }

        public double? AverageScore { get; set; }

        public string? AdressStudent { get; set; }

        public bool? IsMarried { get; set; }

        public bool? ExtendedFamily { get; set; }
    }
}
