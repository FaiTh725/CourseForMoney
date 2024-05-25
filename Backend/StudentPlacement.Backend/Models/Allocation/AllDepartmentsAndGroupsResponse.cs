namespace StudentPlacement.Backend.Models.Allocation
{
    public class AllDepartmentsAndGroupsResponse
    {
        public int IdDepartment { get; set; }

        public string NameDepartment { get; set; }

        public List<SpecializationView> Specializations { get; set; }
    }

    public class SpecializationView
    {
        public int IdSpecialization { get; set; }

        public string ShortName { get; set; }

        public List<GroupsView> Groups { get; set; }
    }
    public class GroupsView
    {
        public int IdGroup { get; set; }

        public string NameGroup { get; set; }
    }
}
