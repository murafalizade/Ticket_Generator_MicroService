using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.API.Services;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        // GET api/<TicketController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<IActionResult> CheckValidationAsync(string id)
        {
            Ticket ticket = await _ticketService.ValidateTicket(id);
            if (ticket == null)
            {
                return BadRequest();
            }
            return Ok(ticket);
        }

        // POST api/<TicketController>
        [HttpPost]
        public async Task<IActionResult> CreateTicketWithoutDesign([FromBody] TicketAttributeDto ticket)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return Forbid();
            }
            byte[] b = await _ticketService.CreateTicketFile(ticket,userId);
            if(b == null)
            {
                return BadRequest("You don't have enough coin");
            }
            return  File(b, "application/octet-stream", "ticket.pdf");
        }

        // PUT api/<TicketController>/5
        [AllowAnonymous]
        [HttpPut("{id}")]
        public IActionResult ConfirmTicket(string id)
        {
            _ticketService.ConfirmTicket(id);
            return NoContent();
        }
    }
}
