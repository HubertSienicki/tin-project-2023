using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [Authorize(Roles = "User, Admin")]
    [HttpGet("user/{userId:int}")]
    public async Task<IActionResult> GetUserOrders(int userId)
    {
        var orders = await _orderInterface.GetUserOrders(userId);
        if (orders.Count == 0)
            return NotFound("No orders found for this user");
        return Ok(orders);
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("{orderId:int}")]
    public async Task<IActionResult> GetOrder(int orderId)
    {
        var order = await _orderInterface.GetOrder(orderId);
        if (order == null)
            return NotFound("Order not found");
        return Ok(order);
    }
    [Authorize(Roles = "User, Admin")]
    [HttpPost("create")]
    public async Task<IActionResult> CreateOrder(int clientId)
    {
        // Validate the user ID
        if (clientId <= 0) return BadRequest("Invalid User ID.");
        
        try
        {
            var createdOrder = await _orderInterface.CreateOrder(clientId);
            // If the order could not be created
            if (createdOrder == null) return BadRequest("Order could not be created.");

            return Ok(createdOrder);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "An error occurred while creating the order.");
        }
    }


    [Authorize(Roles = "User, Admin")]
    [HttpDelete("{orderId:int}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        try
        {
            var orderDeleted = await _orderInterface.DeleteOrder(orderId);
            if (orderDeleted)
                return Ok("Order deleted");

            return NotFound($"Order with ID {orderId} not found.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return StatusCode(500, "An error occurred while deleting the order.");
        }
    }


    // TESTS

    [HttpGet("test")]
    public Task<IActionResult> Test()
    {
        return Task.FromResult<IActionResult>(Ok("Test OK"));
    }

    [Authorize(Roles = "User, Admin")]
    [HttpGet("test/jwt")]
    public IActionResult TestJWT()
    {
        return Ok("JWT is valid!");
    }
}