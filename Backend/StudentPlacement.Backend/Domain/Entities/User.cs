
using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Login {  get; set; }  

        public string Password { get; set; }

        public Role Role { get; set; } = Role.User;

        public string? Token { get; set; } = string.Empty;

        public DateTime? TimeEndToken { get; set; }
    }
}
