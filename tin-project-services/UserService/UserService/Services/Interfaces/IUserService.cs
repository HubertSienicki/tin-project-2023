using UserService.Model;

namespace UserService.Services.Interfaces;

public interface IUserService
{
     public Task<User?>? Authenticate(string username, string password);
}