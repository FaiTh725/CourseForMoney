using StudentPlacement.Backend.Domain.Entities;
using System.Security.Claims;

namespace StudentPlacement.Backend.Services.Interfaces
{
    public interface IJwtProviderService
    {
        (string token, string refreshToken) GenerateTocken(User model);

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
    }
}
