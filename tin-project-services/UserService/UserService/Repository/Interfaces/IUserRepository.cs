using UserService.Model;
using UserService.Model.DTOs;

namespace UserService.Repository.Interfaces;

public interface IUserRepository
{
    public Task<User?>? GetUserById(int id);
}