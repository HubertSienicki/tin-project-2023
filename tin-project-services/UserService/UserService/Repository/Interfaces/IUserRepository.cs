using UserService.Model;

namespace UserService.Repository.Interfaces;

public interface IUserRepository
{
    public Task<User?>? GetUserById(int id);
    public Task<User?> GetByUsername(string username);
}