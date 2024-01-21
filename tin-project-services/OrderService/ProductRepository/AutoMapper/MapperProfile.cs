using AutoMapper;
using OrderService.Model;
using OrderService.Model.DTOs;

namespace OrderService.AutoMapper;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductGet>();
        CreateMap<ProductPost, Product>();
    }
}