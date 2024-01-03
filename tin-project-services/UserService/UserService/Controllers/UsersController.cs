using AutoMapper;
using UserService.Model;
using UserService.Model.DTOs;
using UserService.Repository.Interfaces;
using UserService.Services.Interfaces;

namespace UserService.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IUserService _userService;
    private readonly ITokenService _tokenService;
    private readonly IMapper _mapper;

    public UsersController(IUserRepository userRepository, IUserService userService, ITokenService tokenService, IMapper mapper)
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
        var user = await _userService.Authenticate(loginModel.username, loginModel.password)!;

        if (user == null)
        {
            return Unauthorized();
        }

        var token = _tokenService.GenerateJwtToken(user);
        return Ok(new {token});
    }
}