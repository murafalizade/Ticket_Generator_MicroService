using System.Runtime.InteropServices;
using System.Threading.Tasks;
using QRTicketGenerator.API.Models;

namespace QRTicketGenerator.API.Services
{
    public interface ITicketService
    {
        public Task CreateWithDesign();
        public Task<byte[]> CreateWithoutDesign([Optional] string DelegateName,int EventId);
        public Task<Ticket> ValidateTicket(string id);
        public Task ConfirmTicket(string id);
    }
}
