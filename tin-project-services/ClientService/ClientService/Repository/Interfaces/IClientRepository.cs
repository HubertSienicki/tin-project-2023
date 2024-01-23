using ClientService.Model;

namespace ClientService.Repository.Interfaces;

public interface IClientRepository
{
    public Task<IEnumerable<Client>> GetClientsAsync();
}