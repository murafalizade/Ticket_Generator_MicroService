using System.Runtime.InteropServices;
using System.Threading.Tasks;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;

namespace QRTicketGenerator.API.Services
{
    public interface ITicketService
    {
        public Task<byte[]> CreateTicketFile(TicketAttributeDto ticket,string userId);
        public Task<Ticket> ValidateTicket(string id);
        public Task ConfirmTicket(string id);
    }
}
