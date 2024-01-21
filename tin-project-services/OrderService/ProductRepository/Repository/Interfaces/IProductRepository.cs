using OrderService.Model;
using OrderService.Model.DTOs;

namespace OrderService.Repository.Interfaces;

public interface IProductRepository
{
    public Task<Product?> getProductById(int id);
}