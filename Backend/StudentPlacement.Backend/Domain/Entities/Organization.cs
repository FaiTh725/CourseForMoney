namespace StudentPlacement.Backend.Domain.Entities
{
    public class Organization
    {
        public int Id { get; set; }

        public string Contacts { get; set; } = string.Empty;

        public string Name { get; set; }

        public int? AllocationRequestId { get; set; }

        public AllocationRequest? AllocationRequest { get; set; }

        public int UserId { get; set; }

        public User User { get; set; }
    }
}
