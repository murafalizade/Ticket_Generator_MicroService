using AutoMapper;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;

namespace QRTicketGenerator.API.Mapper
{
    public class AutoMapper:Profile
    {
        public AutoMapper()
        {
            CreateMap<TicketDesignDto,TicketDesign>().ReverseMap();
            CreateMap<UpdateTicketDesignDto,TicketDesignDto>().ReverseMap();
        }
    }
}
