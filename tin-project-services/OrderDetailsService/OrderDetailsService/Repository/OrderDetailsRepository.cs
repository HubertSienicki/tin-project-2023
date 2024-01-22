using MySqlConnector;
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
            const string sql = "SELECT * FROM OrderDetails";
            var command = new MySqlCommand(sql, connection);
            var reader = await command.ExecuteReaderAsync();
            var orderDetails = new List<OrderDetailsGet>();
            while (await reader.ReadAsync())
            {
                var orderDetail = new OrderDetailsGet
                {
                    Quantity = reader.GetInt32(2),
                    AdditionalColumn = reader.GetString(3),
                    OrderId = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1)
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

    public async Task<IEnumerable<OrderDetailsGet>?> GetOrderDetailsByIdAsync(int orderId)
    {
        // out of range index
        if (orderId <= 0) return null;
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            await connection.OpenAsync();
            const string sql = "SELECT * FROM OrderDetails WHERE order_id = @orderId";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@orderId", orderId);
            var reader = await command.ExecuteReaderAsync();
            var orderDetails = new List<OrderDetailsGet>();
            while (await reader.ReadAsync())
            {
                var orderDetail = new OrderDetailsGet
                {
                    Quantity = reader.GetInt32(2),
                    AdditionalColumn = reader.GetString(3),
                    OrderId = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1)
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
                    Quantity = reader.GetInt32(2),
                    AdditionalColumn = reader.GetString(3),
                    OrderId = reader.GetInt32(0),
                    ProductId = reader.GetInt32(1)
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

    public async Task<OrderDetailsGet?> CreateOrderDetailsAsync(OrderDetailsPost orderDetailsPost)
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
            return new OrderDetailsGet
            {
                OrderId = orderDetailsPost.OrderId,
                ProductId = orderDetailsPost.ProductId,
                Quantity = orderDetailsPost.Quantity,
                AdditionalColumn = orderDetailsPost.AdditionalColumn
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
    
    public async Task<OrderDetailsGet?> UpdateOrderDetailsAsync(int orderId, int productId, OrderDetailsPut orderDetailsPut)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));

        try
        {
            await connection.OpenAsync();
            const string sql = "UPDATE OrderDetails SET quantity = @Quantity, additional_column = @AdditionalColumn WHERE order_id = @Id and product_id = @ProductId";
            var command = new MySqlCommand(sql, connection);
            command.Parameters.AddWithValue("@Id", orderId);
            command.Parameters.AddWithValue("@ProductId", productId);
            command.Parameters.AddWithValue("@Quantity", orderDetailsPut.Quantity);
            command.Parameters.AddWithValue("@AdditionalColumn", orderDetailsPut.AdditionalColumn);
            await command.ExecuteNonQueryAsync();
            return new OrderDetailsGet
            {
                OrderId = orderId,
                ProductId = productId,
                Quantity = orderDetailsPut.Quantity,
                AdditionalColumn = orderDetailsPut.AdditionalColumn
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
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
}