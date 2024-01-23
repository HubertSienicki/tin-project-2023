using OrderDetailsService.Model.DTOs;

namespace OrderDetailsService.Repository.Interfaces;

public interface IOrderDetailsRepository
{
    public Task<IEnumerable<OrderDetailsGet>> GetOrderDetailsAsync(int pageNumber, int pageSize);
    public Task<bool> CreateOrderDetailsAsync(OrderDetailsPost orderDetailsPost);
    public Task<bool> DeleteOrderDetailsAsync(int orderId);
    public Task<bool> UpdateOrderDetailsAsync(int orderId, OrderDetailsPut orderDetailsPut);
}