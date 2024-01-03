using UserService.Model;

namespace UserService.Services.Interfaces;

public interface ITokenService
{
    public string GenerateJwtToken(User user);
}