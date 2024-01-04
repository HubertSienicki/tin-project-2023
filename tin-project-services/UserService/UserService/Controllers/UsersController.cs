using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Model;
using UserService.Model.DTOs;
using UserService.Repository.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IUserService userService, ITokenService tokenService,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _userService = userService;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id)!;
        if (user == null) return NotFound("User not found");

        var userDto = _mapper.Map<UserGet>(user);
        return Ok(userDto);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
    {
        var user = await _userService.Authenticate(loginModel.Username, loginModel.Password)!;

        if (user == null) return Unauthorized();

        var token = _tokenService.GenerateJwtToken(user);
        return Ok(new { token });
    }

    [HttpPost("register")]
    public Task<IActionResult> Register([FromBody] RegisterModel registerModel)
    {
        var salt = _userService.HashPassword(registerModel);
        
        // map user from RegisterModel
        var user = _mapper.Map<UserPost>(registerModel);

        // add new user
        var actionResult = _userRepository.AddNewUser(user, salt);

        // return result
        return actionResult.Result == 1 ? Task.FromResult<IActionResult>(Ok(user)) : Task.FromResult<IActionResult>(BadRequest());
    }
}