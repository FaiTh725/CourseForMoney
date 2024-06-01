using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Models.Allocation
{
    public class GetStudentsFromRequestResponse
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public double AverageScore { get; set; }

        public string Adress { get; set; } = string.Empty;

        public bool IsMarried { get; set; }

        public bool ExtendedFamily { get; set; }
    }
}
