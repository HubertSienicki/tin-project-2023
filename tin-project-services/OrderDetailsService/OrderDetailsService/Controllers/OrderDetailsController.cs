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
    public async Task<IActionResult> GetOrderDetailsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var orderDetails = await _orderDetailsRepository.GetOrderDetailsAsync(pageNumber, pageSize);
        return Ok(orderDetails);
    }

    [Authorize(Roles = "Admin, User")]
    [HttpPost("create")]
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
    
    [Authorize(Roles = "Admin")]
    [HttpPut("update/{orderId:int}")]
    public Task<IActionResult> UpdateOrderDetailsAsync(int orderId, OrderDetailsPut orderDetailsPut)
    {
        // validate json schema
        var validationResult = _orderDetailsService.ValidateJsonSchema(orderDetailsPut, "PUT");
        if (!validationResult.Item1) return Task.FromResult<IActionResult>(BadRequest(validationResult.Item2));

        var orderDetails = _orderDetailsRepository.UpdateOrderDetailsAsync(orderId, orderDetailsPut);
        return orderDetails.Result == null
            ? Task.FromResult<IActionResult>(BadRequest("Order details could not be updated"))
            : Task.FromResult<IActionResult>(Ok(orderDetails.Result));
    }

    [HttpGet("test")]
    public Task<IActionResult> Test()
    {
        return Task.FromResult<IActionResult>(Ok("Test OK"));
    }
}