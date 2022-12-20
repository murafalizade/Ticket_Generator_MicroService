using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.Shared;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public interface ITicketDesignService
    {
        public Task<ResponseDto<List<TicketDesign>>> GetAll(string userId);
        public Task<ResponseDto<TicketDesign>> GetById(string id,string userId);
        public Task<ResponseDto<string>> Create(TicketDesignDto entity,string userId);
        public Task<ResponseDto<NoContent>> Delete(string id, string userId);
        public Task<ResponseDto<NoContent>> Update(UpdateTicketDesignDto entity,string userId);
    }
}
