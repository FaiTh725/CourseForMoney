
using StudentPlacement.Backend.Domain.Enums;

namespace StudentPlacement.Backend.Domain.Entities
{
    public class User
    {
        public int Id { get; set; }

        public string Login {  get; set; }  

        public string Password { get; set; }

        public Role Role { get; set; } = Role.User;

        public string? ImageUserStringFormat {  get; set; } // если аватарки нету то аватарку по стандарту хранит путь к файла в папке StorageUserImage
        
        public string Email {  get; set; }

        public string? Token { get; set; } = string.Empty;

        public DateTime? TimeEndToken { get; set; }
    }
}
