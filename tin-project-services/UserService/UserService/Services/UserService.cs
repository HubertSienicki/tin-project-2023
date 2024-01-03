using System.Security.Cryptography;
using System.Text;
using UserService.Model;
using UserService.Repository.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;
    

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<User?> Authenticate(string username, string password)
    {
        var user = _userRepository.GetByUsername(username);

        if (user.Result?.Password != null && ValidatePassword(password, user.Result.Password))
        {
            return await user ?? throw new InvalidOperationException();
        }

        return null;
    }

    private static bool ValidatePassword(string password, string hashedPassword)
    {
        // hash incomming password
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        //verify hashes
        return hashString == hashedPassword;
    }
}