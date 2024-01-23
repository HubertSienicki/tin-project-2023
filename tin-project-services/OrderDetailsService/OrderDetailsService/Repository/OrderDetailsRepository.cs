using MySqlConnector;
using OrderDetailsService.Model;
using OrderDetailsService.Model.DTOs;
using OrderDetailsService.Repository.Interfaces;

namespace OrderDetailsService.Repository;

public class OrderDetailsRepository : IOrderDetailsRepository
{
    private readonly IConfiguration _configuration;

    public OrderDetailsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    
    public async Task<IEnumerable<OrderDetailsGet>> GetOrderDetailsAsync()
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            await connection.OpenAsync();
            const string sql = "SELECT order_id, order_date, product_id, quantity, additional_column, p1.name as product_name, p1.price as product_price, U.Id as client_id, U.first_name as first_name, U.last_name as last_name, U.email as user_email, U.phone as phone FROM OrderDetails od1 JOIN Products p1 ON od1.product_id = p1.id JOIN mydb.Orders O on od1.order_id = O.id join mydb.Clients U on U.id = O.client_id";
            var command = new MySqlCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();
            var orderDetails = new List<OrderDetailsGet>();
            
            while (await reader.ReadAsync())
            {
                var orderDetail = new OrderDetailsGet
                {
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Product = await GetProductsForOrder(reader.GetInt32("order_id")),
                    AdditionalColumn = reader.GetString(reader.GetOrdinal("additional_column")),
                    Order = new OrderGet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("order_id")),
                        Date = reader.GetDateTime(reader.GetOrdinal("order_date")),
                        Client = new Client
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("client_id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("user_email")),
                            Phone = reader.GetString(reader.GetOrdinal("phone"))
                        }
                    }
                };
                orderDetails.Add(orderDetail);
            }
            return orderDetails;
        }catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<OrderDetailsGet> GetOrderDetailsByIdAsync(int orderId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT * FROM OrderDetails od1 JOIN Products p1 ON od1.product_id = p1.id JOIN mydb.Orders O on od1.order_id = O.id join mydb.Clients U on U.id = O.client_id where od1.order_id = {orderId}";

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                var orderDetail = new OrderDetailsGet
                {
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Product = await GetProductsForOrder(orderId),
                    AdditionalColumn = reader.GetString(reader.GetOrdinal("additional_column")),
                    Order = new OrderGet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Date = reader.GetDateTime(reader.GetOrdinal("order_date")),
                        Client = new Client
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("user_email")),
                            Phone = reader.GetString(reader.GetOrdinal("phone"))
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

    public async Task<IEnumerable<OrderDetailsGet>?> GetOrderDetailsByProductIdAsync(int productId)
    {
        if (productId <= 0) return null;
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            await connection.OpenAsync();
            const string sql = "SELECT * FROM OrderDetails WHERE product_id = @Id";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", productId);
            var reader = await command.ExecuteReaderAsync();
            var orderDetails = new List<OrderDetailsGet>();
            while (await reader.ReadAsync())
            {
                var orderDetail = new OrderDetailsGet
                {
                    Quantity = reader.GetInt32(reader.GetOrdinal("quantity")),
                    Product = await GetProductsForOrder(reader.GetOrdinal("order_id")),
                    AdditionalColumn = reader.GetString(reader.GetOrdinal("additional_column")),
                    Order = new OrderGet
                    {
                        Id = reader.GetInt32(reader.GetOrdinal("id")),
                        Date = reader.GetDateTime(reader.GetOrdinal("order_date")),
                        Client = new Client
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            FirstName = reader.GetString(reader.GetOrdinal("first_name")),
                            LastName = reader.GetString(reader.GetOrdinal("last_name")),
                            Email = reader.GetString(reader.GetOrdinal("email")),
                            Phone = reader.GetString(reader.GetOrdinal("phone"))
                        }
                    }
                };
                orderDetails.Add(orderDetail);
            }
            return orderDetails;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<bool> CreateOrderDetailsAsync(OrderDetailsPost orderDetailsPost)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            await connection.OpenAsync();
            const string sql = "INSERT INTO OrderDetails (order_id, product_id, quantity, additional_column) VALUES (@OrderId, @ProductId, @Quantity, @AdditionalColumn)";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@OrderId", orderDetailsPost.OrderId);
            command.Parameters.AddWithValue("@ProductId", orderDetailsPost.ProductId);
            command.Parameters.AddWithValue("@Quantity", orderDetailsPost.Quantity);
            command.Parameters.AddWithValue("@AdditionalColumn", orderDetailsPost.AdditionalColumn);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }
    public async Task<bool> DeleteOrderDetailsAsync(int id)
    {   
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            await connection.OpenAsync();
            const string sql = "DELETE FROM OrderDetails WHERE order_id = @Id";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", id);
            await command.ExecuteNonQueryAsync();
            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    private async Task<List<ProductGet>> GetProductsForOrder(int orderId)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();
            await using var command = connection.CreateCommand();
            command.CommandText =
                $"SELECT p1.id,p1.price,p1.name FROM OrderDetails od1 JOIN Products p1 ON od1.product_id = p1.id JOIN mydb.Orders O on od1.order_id = O.id join mydb.Clients U on U.id = O.client_id where od1.order_id = {orderId}";

            Console.WriteLine(command.CommandText);

            await using var reader = await command.ExecuteReaderAsync();
            var products = new List<ProductGet>();
            while (await reader.ReadAsync())
            {
                var product = new ProductGet
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Price = reader.GetDecimal(reader.GetOrdinal("price")),
                    Name = reader.GetString(reader.GetOrdinal("name"))
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