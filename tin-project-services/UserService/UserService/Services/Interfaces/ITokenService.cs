using UserService.Model;
using UserService.Model.DTOs;

namespace UserService.Services.Interfaces;

public interface ITokenService
{
    public string GenerateJwtToken(UserLogon user);
}