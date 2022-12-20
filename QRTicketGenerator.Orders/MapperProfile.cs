using AutoMapper;
using QRTicketGenerator.Orders.Models;
class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductGetDto>().ReverseMap();
        CreateMap<ProductsCreateDto, Product>();
        CreateMap<Order, OrderCreateDto>().ReverseMap();
        CreateMap<Order, OrderGetDto>().ReverseMap();
    }
}