using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OrderDetailsService.Model.DTOs;
using OrderDetailsService.OrderDetailsService.Interfaces;
using OrderDetailsService.Repository.Interfaces;

namespace OrderDetailsService.Controllers;

[ApiController]
[Route("Order/details")]
public class OrderDetailsController : ControllerBase
{
    private readonly IOrderDetailsRepository _orderDetailsRepository;
    private readonly IOrderDetailsService _orderDetailsService;

    public OrderDetailsController(IOrderDetailsRepository orderDetailsRepository,
        IOrderDetailsService orderDetailsService)
    {
        _orderDetailsRepository = orderDetailsRepository;
        _orderDetailsService = orderDetailsService;
    }
    
    [Authorize(Roles = "Admin")]
    [HttpGet("")]
    public Task<IActionResult> GetOrderDetailsAsync()
    {
        var orderDetails = _orderDetailsRepository.GetOrderDetailsAsync();
        return Task.FromResult<IActionResult>(Ok(orderDetails.Result));
    }
    
    [Authorize(Roles = "Admin, User")]
    [HttpGet("{orderid:int}")]
    public Task<IActionResult> GetOrderDetailsByIdAsync(int orderid)
    {
        var orderDetails = _orderDetailsRepository.GetOrderDetailsByIdAsync(orderid);
        return Task.FromResult<IActionResult>(Ok(orderDetails.Result));
    }
    [Authorize(Roles = "Admin, User")]
    [HttpGet("product/{productid:int}")]
    public Task<IActionResult> GetOrderDetailsByProductIdAsync(int productid)
    {
        var orderDetails = _orderDetailsRepository.GetOrderDetailsByProductIdAsync(productid);
        return orderDetails.Result != null && !orderDetails.Result.Any()
            ? Task.FromResult<IActionResult>(NotFound("No orders found for this given product id"))
            : Task.FromResult<IActionResult>(Ok(orderDetails.Result));
    }
    [Authorize(Roles = "Admin, User")]
    [HttpPost("")]
    public Task<IActionResult> CreateOrderDetailsAsync([FromBody] OrderDetailsPost orderDetailsPost)
    {
        // validate json schema
        var validationResult = _orderDetailsService.ValidateJsonSchema(orderDetailsPost, "POST");
        if (!validationResult.Item1) return Task.FromResult<IActionResult>(BadRequest(validationResult.Item2));

        var orderDetails = _orderDetailsRepository.CreateOrderDetailsAsync(orderDetailsPost);
        return orderDetails.Result == null
            ? Task.FromResult<IActionResult>(BadRequest("Order details could not be created"))
            : Task.FromResult<IActionResult>(Ok(orderDetails.Result));
    }
    
    [Authorize(Roles = "Admin")]
    [HttpDelete("{id:int}")]
    public Task<IActionResult> DeleteOrderDetailsAsync(int id)
    {
        var orderDetails = _orderDetailsRepository.DeleteOrderDetailsAsync(id);
        return orderDetails.Result
            ? Task.FromResult<IActionResult>(Ok("Order details deleted"))
            : Task.FromResult<IActionResult>(BadRequest("Order details could not be deleted"));
    }
    
    [HttpGet("test")]
    public Task<IActionResult> Test()
    {
        return Task.FromResult<IActionResult>(Ok("Test OK"));
    }
}