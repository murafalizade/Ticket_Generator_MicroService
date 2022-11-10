using MongoDB.Driver;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QRTicketGenerator.API.Services
{
    public class EventService : IEventService
    {
        private readonly IMongoCollection<Event> _events;

        public EventService(ITicketDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            _events = database.GetCollection<Event>("Events");
        }
        public async Task Create(Event obj)
        {
            await _events.InsertOneAsync(obj);
        }

        public async Task Delete(string id,string userId)
        {
            await _events.DeleteOneAsync(x=>x.Id == id && x.UserId == userId);
        }

        public async Task<List<Event>> Get(string userId)
        {
            List<Event> events = await _events.Find(p=>p.UserId == userId).ToListAsync();
            return events;
        }
    }
}
