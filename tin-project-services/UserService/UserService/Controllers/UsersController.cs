using AutoMapper;
using UserService.Model.DTOs;
using UserService.Repository.Interfaces;

namespace UserService.Controllers;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    
    public UsersController(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
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
}