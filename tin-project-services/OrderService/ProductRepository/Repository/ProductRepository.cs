using AutoMapper;
using MySqlConnector;
using OrderService.Model;
using OrderService.Model.DTOs;
using OrderService.Repository.Interfaces;

namespace OrderService.Repository;

public class ProductRepository : IProductRepository
{
    private readonly IConfiguration _configuration;

    public ProductRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<Product?> getProductById(int id)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            command.CommandText = $"SELECT * FROM Products WHERE id = {id}";
            await using var reader = await command.ExecuteReaderAsync();

            if (reader.Read())
            {
                var product = new Product
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Price = reader.GetDecimal("price")
                };
                return product;
            }
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }
}