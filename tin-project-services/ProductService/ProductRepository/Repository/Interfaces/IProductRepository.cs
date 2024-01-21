using OrderService.Model;
using OrderService.Model.DTOs;

namespace OrderService.Repository.Interfaces;

public interface IProductRepository
{
    public Task<Product?> GetProductById(int id);
    public Task<List<Product>> GetAllProducts();
    public Task<Product?> AddProduct(Product? product);
    public Task<Product?> UpdateProduct(Product? product);
}