using System.Collections.Generic;
using System.Threading.Tasks;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.Shared;

namespace QRTicketGenerator.API.Services
{
    public interface IEventService
    {
        public Task<ResponseDto<List<Event>>> Get(string userId);
        public Task<ResponseDto<NoContent>> Create(CreateEventDto obj,string userId);
        public Task<ResponseDto<NoContent>> Delete(string id,string userId);
    }
}
