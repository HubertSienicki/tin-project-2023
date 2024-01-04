using UserService.Model;
using UserService.Model.DTOs;

namespace UserService.Services.Interfaces;

public interface IUserService
{
     public Task<UserLogon?>? Authenticate(string username, string password);
     public string HashPassword(RegisterModel registerModel);
}