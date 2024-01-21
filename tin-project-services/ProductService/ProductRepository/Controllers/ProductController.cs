using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Model;
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
        var product = await _productRepository.GetProductById(id);
        if (product == null) return NotFound("Product not available");

        var productDto = _mapper.Map<ProductGet>(product);
        return Ok(productDto);
    }
    
    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        var products = await _productRepository.GetAllProducts();
        if (products == null) return NotFound("No products available");

        var productsDto = _mapper.Map<List<ProductGet>>(products);
        return Ok(productsDto);
    }
    
    [HttpPost]
    public async Task<IActionResult> AddProduct(ProductPost productPost)
    {
        var product = _mapper.Map<Product>(productPost);
        var addedProduct = await _productRepository.AddProduct(product);
        if (addedProduct == null) return BadRequest("Product not added");

        var productDto = _mapper.Map<ProductGet>(addedProduct);
        return Ok(productDto);
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateProduct(ProductPut productPut)
    {
        var product = _mapper.Map<Product>(productPut);
        var updatedProduct = await _productRepository.UpdateProduct(product);
        if (updatedProduct == null) return BadRequest("Product not updated");

        var productDto = _mapper.Map<ProductGet>(updatedProduct);
        return Ok(productDto);
    }
}