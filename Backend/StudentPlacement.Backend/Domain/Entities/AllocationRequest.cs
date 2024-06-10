namespace StudentPlacement.Backend.Domain.Entities
{
    // я долбаеб и в некоторых случаях можно следать так что бы при удалении заявки у студента она становилась null используя ondelete.setnull 
    public class AllocationRequest
    {
        public int Id { get; set; }

        public string Adress { get; set; } = string.Empty;

        public string Specialist { get; set; } = string.Empty ;

        public int CountPlace { get; set; }

        public string OrderFilePath { get; set; } = string.Empty;

        public List<Student> Students { get; set; } = new List<Student>();

        public int OrganizationId {  get; set; }

        public Organization Organization { get; set; }
    }
}
