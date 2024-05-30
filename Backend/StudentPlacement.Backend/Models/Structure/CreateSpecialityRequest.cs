namespace StudentPlacement.Backend.Models.Structure
{
    public class CreateSpecialityRequest
    {
        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Code { get; set; }

        public int IdDepartment { get; set; }
    }
}
