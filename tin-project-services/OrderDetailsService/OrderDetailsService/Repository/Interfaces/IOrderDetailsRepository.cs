using OrderDetailsService.Model.DTOs;

namespace OrderDetailsService.Repository.Interfaces;

public interface IOrderDetailsRepository
{
    public Task<IEnumerable<OrderDetailsGet>> GetOrderDetailsAsync();
    public Task<IEnumerable<OrderDetailsGet>?> GetOrderDetailsByIdAsync(int orderId);
    public Task<IEnumerable<OrderDetailsGet>?> GetOrderDetailsByProductIdAsync(int productId);
    public Task<OrderDetailsGet?> CreateOrderDetailsAsync(OrderDetailsPost orderDetailsPost);
    public Task<OrderDetailsGet?> UpdateOrderDetailsAsync(int id, OrderDetailsPost orderDetailsPost);
    public Task<bool> DeleteOrderDetailsAsync(int id);
}