using System.Security.Cryptography;
using System.Text;
using UserService.Model;
using UserService.Model.DTOs;
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

    public async Task<UserLogon?> Authenticate(string username, string password)
    {
        var user = _userRepository.GetByUsername(username);
        var saltedPassword = password + user.Result?.Salt; // salt incoming password

        // when user has password and it validates return user
        if (user.Result?.Password != null && ValidatePassword(saltedPassword, user.Result.Password))
            return await user ?? throw new InvalidOperationException();

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

    public string HashPassword(RegisterModel registerModel)
    {
        //Salt password
        var password = registerModel.Password;
        var salt = GenerateSalt();

        var saltedUserPassword = password + salt;

        // hash incomming password
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(saltedUserPassword));
        var hashString = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        registerModel.Password = hashString;

        return salt;
    }

    /***
     * Random salt generation
     */
    private static string GenerateSalt()
    {
        var randomBytes = new byte[15];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomBytes);
        }

        return Convert.ToBase64String(randomBytes);
    }
}