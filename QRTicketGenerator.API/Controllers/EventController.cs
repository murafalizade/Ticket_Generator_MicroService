using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.API.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventRepository;
        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventRepository, ILogger<EventController> logger)
        {
            _eventRepository = eventRepository;
            _logger = logger;
        }
        // GET: api/<EventController
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            string userId = "string";
            return Ok(await _eventRepository.Get(userId));
        }

        // POST api/<EventController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Event value)
        {
            await _eventRepository.Create(value);
            return Ok();
        }

        // DELETE api/<EventController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            string userId = "string";
            await _eventRepository.Delete(id,userId);
           return Ok();
        }
    }
}
