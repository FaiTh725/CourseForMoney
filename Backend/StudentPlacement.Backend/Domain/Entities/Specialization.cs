namespace StudentPlacement.Backend.Domain.Entities
{
    public class Specialization
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string ShortName { get; set; } = string.Empty;

        public string Code { get; set; } = string.Empty;
    
        public int DepartmentId { get; set; }

        public Department Department { get; set; }

        public List<Group> Groups { get; set; }
        /*public int GroupId { get; set; }

        public Group Group { get; set; }*/
    }
}
