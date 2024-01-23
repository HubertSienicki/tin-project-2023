using OrderService.Model.DTOs;

namespace OrderService.Repository.Interfaces;

public interface IOrderInterface
{
    public Task<List<OrderGet>> GetUserOrders(int userId);
    public Task<OrderGet?> GetOrder(int orderId);
    public Task<OrderCreate> CreateOrder(int userId);
    public Task<bool> DeleteOrder(int orderId);
}