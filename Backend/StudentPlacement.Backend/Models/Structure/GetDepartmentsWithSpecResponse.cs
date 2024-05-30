namespace StudentPlacement.Backend.Models.Structure
{
    public class GetDepartmentsWithSpecResponse
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public List<GetAllOptionsResponse> DepartmentSpeciality { get; set; } = new List<GetAllOptionsResponse>();
    }
}
