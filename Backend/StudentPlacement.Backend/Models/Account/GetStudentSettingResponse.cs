namespace StudentPlacement.Backend.Models.Account
{
    public class GetStudentSettingResponse
    {
        public List<GroupView> Groups { get; set; } = new();

    }

    public class GroupView
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
