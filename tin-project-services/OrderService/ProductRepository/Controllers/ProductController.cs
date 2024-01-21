using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model.DTOs;
using OrderService.Repository.Interfaces;

namespace OrderService.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public ProductController(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productRepository.getProductById(id);
        if (product == null) return NotFound("Product not available");

        var productDto = _mapper.Map<ProductGet>(product);
        return Ok(productDto);
    }
}