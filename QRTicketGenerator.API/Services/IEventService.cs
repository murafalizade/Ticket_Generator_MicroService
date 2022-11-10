using System.Collections.Generic;
using System.Threading.Tasks;
using QRTicketGenerator.API.Models;

namespace QRTicketGenerator.API.Services
{
    public interface IEventService
    {
        public Task<List<Event>> Get(string userId);
        public Task Create(Event obj);
        public Task Delete(string id,string userId);
    }
}
