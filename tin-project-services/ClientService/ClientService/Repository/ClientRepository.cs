using ClientService.Model;
using ClientService.Model.DTOs;
using ClientService.Repository.Interfaces;
using MySqlConnector;

namespace ClientService.Repository;

public class ClientRepository : IClientRepository
{
    private readonly IConfiguration _configuration;
    
    public ClientRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<IEnumerable<Client>> GetClientsAsync()
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            await connection.OpenAsync();
            const string sql = "SELECT * FROM Clients";
            var command = new MySqlCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();
            var clients = new List<Client>();
            
            while (await reader.ReadAsync())
            {
                var client = new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName = reader.GetString(reader.GetOrdinal("last_name")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Phone = reader.GetString(reader.GetOrdinal("phone"))
                };
                clients.Add(client);
            }
            return clients;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<ClientPOST> AddClientAsync(ClientPOST clientPost)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            await connection.OpenAsync();
            const string sql = "INSERT INTO Clients (first_name, last_name, email, phone) VALUES (@FirstName, @LastName, @Email, @Phone)";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@FirstName", clientPost.FirstName);
            command.Parameters.AddWithValue("@LastName", clientPost.LastName);
            command.Parameters.AddWithValue("@Email", clientPost.Email);
            command.Parameters.AddWithValue("@Phone", clientPost.Phone);
            await command.ExecuteNonQueryAsync();
            return clientPost;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}