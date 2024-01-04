using UserService.Model;
using UserService.Model.DTOs;

namespace UserService.Repository.Interfaces;

public interface IUserRepository
{
    public Task<User?>? GetUserById(int id);
    public Task<UserLogon?> GetByUsername(string username);
    public Task<int> AddNewUser(UserPost userPost, string passwordSalt);
}