using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Services;
using QRTicketGenerator.Shared.ControllerBases;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EventController : CustomControllerBase
    {
        private readonly IEventService _eventRepository;

        public EventController(IEventService eventRepository)
        {
            _eventRepository = eventRepository;
        }
        // GET: api/<EventController
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type ==  ClaimTypes.NameIdentifier).Value;
            var response = await _eventRepository.Get(userId.ToString());
            return CreateActionResultInstance(response);
        }

        // POST api/<EventController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateEventDto value)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type ==  ClaimTypes.NameIdentifier).Value;
            return CreateActionResultInstance(await _eventRepository.Create(value, userId));
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type ==  ClaimTypes.NameIdentifier).Value;
            return CreateActionResultInstance(await _eventRepository.Delete(id, userId));
        }
    }
}
