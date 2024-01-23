using ClientService.Model;
using ClientService.Model.DTOs;

namespace ClientService.Repository.Interfaces;

public interface IClientRepository
{
    public Task<IEnumerable<Client>> GetClientsAsync();
    public Task<ClientPOST> AddClientAsync(ClientPOST clientPost);
}