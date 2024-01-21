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

    public async Task<Product?> GetProductById(int id)
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
    
    public async Task<List<Product>> GetAllProducts()
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        // write code to get all products from database
        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            command.CommandText = "SELECT * FROM Products";
            await using var reader = await command.ExecuteReaderAsync();
            
            var products = new List<Product>();
            // read all products from database
            while (reader.Read())
            {
                var product = new Product
                {
                    Id = reader.GetInt32("id"),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Price = reader.GetDecimal("price")
                };
                products.Add(product);
            }

            return products;
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null;
    }

    public async Task<Product?> AddProduct(Product? product)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            if (product != null)
            {
                command.CommandText =
                    $"INSERT INTO Products (name, price) VALUES ('{product?.Name}', {product!.Price})";
                await command.ExecuteNonQueryAsync();
                product.Id = (int)command.LastInsertedId;
                return product;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
        return null;
    }

    public async Task<Product?> UpdateProduct(Product? product)
    {
        await using var connection = new MySqlConnection(_configuration.GetConnectionString("DefaultConnection"));
        try
        {
            connection.Open();

            await using var command = connection.CreateCommand();
            if (product != null)
            {
                command.CommandText =
                    $"UPDATE Products SET name = '{product?.Name}', price = {product!.Price} WHERE id = {product.Id}";
                await command.ExecuteNonQueryAsync();
                return product;
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }

        return null!;
    }
}