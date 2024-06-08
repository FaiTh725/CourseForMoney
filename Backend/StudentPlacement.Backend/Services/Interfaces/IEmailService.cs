namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendDefaultEmail(string email, string subject, string message);
    }
}
