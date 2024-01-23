using ClientService.Repository.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ClientService.Controllers;

[ApiController]
[Route(("[controller]"))]
public class ClientController : ControllerBase
{
    private readonly IClientRepository _clientRepository;

    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    
    [Authorize (Roles = "Admin, User")]
    [HttpGet]
    public Task<IActionResult> GetClientsAsync()
    {
        var clients = _clientRepository.GetClientsAsync();
        return Task.FromResult<IActionResult>(Ok(clients.Result));
    }
}