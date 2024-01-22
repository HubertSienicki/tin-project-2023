using Microsoft.AspNetCore.Mvc;
using OrderService.Model.DTOs;
using OrderService.Repository.Interfaces;

namespace OrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderController : ControllerBase
{
    private readonly IOrderInterface _orderInterface;

    public OrderController(IOrderInterface orderInterface)
    {
        _orderInterface = orderInterface;
    }

    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var orders = await _orderInterface.GetUserOrders(userId);
        if (orders.Count == 0)
            return NotFound("No orders found for this user");
        return Ok(orders);
    }

    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await _orderInterface.GetOrder(orderId);
        if (order == null)
            return NotFound("Order not found");
        return Ok(order);
    }

    [HttpGet("{orderId:int}/details")]
    public async Task<IActionResult> GetOrderDetails(int orderId)
    {
        var orderDetails = await _orderInterface.GetOrderDetails(orderId);
        if (orderDetails == null)
            return NotFound("Order not found");
        return Ok(orderDetails);
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(int userId)
    {
        var createdOrder = await _orderInterface.CreateOrder(userId);
        if (createdOrder == null)
            return BadRequest("Order not created");
        return Ok(createdOrder);
    }
    
    [HttpGet("test")]
    public Task<IActionResult> Test()
    {
        return Task.FromResult<IActionResult>(Ok("Test OK"));
    }
}