using MySql.Data.MySqlClient;
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

    public async Task<List<OrderGet>> GetUserOrders(int userId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM Orders o1 JOIN Users u1 ON o1.user_id = u1.id WHERE o1.user_id = {userId}";

            var orders = new List<OrderGet>();

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var order = new OrderGet
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                    User = new UserGet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Username = reader.GetString(reader.GetOrdinal("username")),
                        Email = reader.GetString(reader.GetOrdinal("email"))
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
                $"SELECT * FROM Orders o1 JOIN Users u1 ON o1.user_id = u1.id WHERE o1.id = {orderId}";

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();

            if (!await reader.ReadAsync()) return null;
            var order = new OrderGet
            {
                Id = reader.GetInt32(reader.GetOrdinal("id")),
                OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                User = new UserGet
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Username = reader.GetString(reader.GetOrdinal("username")),
                    Email = reader.GetString(reader.GetOrdinal("email"))
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

    public async Task<OrderDetailsGet> GetOrderDetails(int orderId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM OrderDetails od1 JOIN Products p1 ON od1.product_id = p1.id JOIN mydb.Orders O on od1.order_id = O.id join mydb.Users U on U.id = O.user_id where od1.order_id = {orderId}";

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var orderDetail = new OrderDetailsGet
                {
                    OrderId = reader.GetInt32(reader.GetOrdinal("id")),
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Product = await getProductsForOrder(orderId),
                    AdditionalColumn = reader.GetString(reader.GetOrdinal("additional_column")),
                    Order = new OrderGet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        OrderDate = reader.GetDateTime(reader.GetOrdinal("order_date")),
                        User = new UserGet
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            Username = reader.GetString(reader.GetOrdinal("username")),
                            Email = reader.GetString(reader.GetOrdinal("email"))
                        }
                    }
                };
                return orderDetail;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }

        return null;
    }

    public async Task<OrderCreate> CreateOrder(int userId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"INSERT INTO Orders (user_id, order_date) VALUES ({userId}, '{DateTime.Now:yyyy-MM-dd}')";
            await command.ExecuteNonQueryAsync();
            var orderCreate = new OrderCreate { UserId = userId, OrderDate = DateTime.Now};
            return orderCreate;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }

    public Task UpdateOrder(OrderUpdate order)
    {
        throw new NotImplementedException();
    }

    public Task DeleteOrder(int orderId)
    {
        throw new NotImplementedException();
    }
    
    private async Task<List<ProductGet>> getProductsForOrder(int orderId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM OrderDetails od1 JOIN Products p1 ON od1.product_id = p1.id JOIN mydb.Orders O on od1.order_id = O.id join mydb.Users U on U.id = O.user_id where od1.order_id = {orderId}";

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();
            var products = new List<ProductGet>();
            while (await reader.ReadAsync())
            {
                var product = new ProductGet
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    price = reader.GetDecimal(reader.GetOrdinal("price")),
                    name = reader.GetString(reader.GetOrdinal("name"))
                };
                products.Add(product);
            }

            return products;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            throw;
        }
    }
}