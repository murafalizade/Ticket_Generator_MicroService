using AutoMapper;
using QRTicketGenerator.Orders.Models;
class MapperProfile:Profile
{
    public MapperProfile()
    {
        CreateMap<Product, ProductGetDto>().ReverseMap();
        CreateMap<Order, OrderCreateDto>().ReverseMap();
        CreateMap<Order, OrderGetDto>().ReverseMap();
    }
}