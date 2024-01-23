using MySql.Data.MySqlClient;
using OrderService.Model;
using OrderService.Model.DTOs;
using OrderService.Repository.Interfaces;

namespace OrderService.Repository;

public class OrderInterface : IOrderInterface
{
    private readonly IConfiguration _configuration;

    public OrderInterface(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<List<OrderGet>> GetUserOrders(int clientId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM Orders o1 JOIN Clients u1 ON o1.client_id = u1.id WHERE o1.client_id = {clientId}";

            var orders = new List<OrderGet>();

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var order = new OrderGet
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                    Client = new Client
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                        LastName = reader.GetString(reader.GetOrdinal("last_name")),
                        Email = reader.GetString(reader.GetOrdinal("email")),
                        Phone = reader.GetString(reader.GetOrdinal("phone"))
                    }
                };
                orders.Add(order);
            }

            return orders;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public async Task<OrderGet?> GetOrder(int orderId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM Orders o1 JOIN Clients u1 ON o1.client_id = u1.id WHERE o1.id = {orderId}";

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;
            var order = new OrderGet
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                Client = new Client
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                    LastName = reader.GetString(reader.GetOrdinal("last_name")),
                    Email = reader.GetString(reader.GetOrdinal("email")),
                    Phone = reader.GetString(reader.GetOrdinal("phone"))
                }
            };
            return order;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    public async Task<OrderCreate> CreateOrder(int clientId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                command.CommandText = $"INSERT INTO Orders (client_id, order_date) VALUES ({clientId}, '{DateTime.Now:yyyy-MM-dd}'); SELECT LAST_INSERT_ID();";
            
            // return the id of the newly created order
            var orderId = Convert.ToInt32(await command.ExecuteScalarAsync());
            
            var orderCreate = new OrderCreate { OrderId = orderId, ClientId = clientId, OrderDate = DateTime.Now};
            return orderCreate;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
    public async Task<bool> DeleteOrder(int orderId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText = $"DELETE FROM Orders WHERE id = {orderId}";
            var result = await command.ExecuteNonQueryAsync();
            return result == 1;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}