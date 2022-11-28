using MongoDB.Driver;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.Shared;
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

        public async Task<ResponseDto<NoContent>> Create(CreateEventDto obj,string userId)
        {
            Event e = new Event() { Name = obj.Name, UserId = userId };
            await _events.InsertOneAsync(e);
            return  ResponseDto<NoContent>.Success(new NoContent(),204);
        }

        public async Task<ResponseDto<NoContent>> Delete(string id,string userId)
        {
            if(id == null)
            {
                return ResponseDto<NoContent>.Fail("Event Id is null", 403);
            }
            if(userId == null)
            {
                return ResponseDto<NoContent>.Fail("User is null", 401);
            }
            await _events.DeleteOneAsync(x=>x.Id == id && x.UserId == userId);
            return ResponseDto<NoContent>.Success(new NoContent(), 204);
        }

        public async Task<ResponseDto<List<Event>>> Get(string userId)
        {
            if (userId == null)
            {
                return ResponseDto<List<Event>>.Fail("User is null", 401);
            }
            List<Event> events = await _events.Find(p=>p.UserId == userId).ToListAsync();
            return ResponseDto<List<Event>>.Success(events,200);
        }
    }
}
