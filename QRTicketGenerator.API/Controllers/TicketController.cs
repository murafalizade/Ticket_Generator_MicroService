using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.API.Services;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        // GET api/<TicketController>/5
        [HttpGet("{id}")]
        public string CheckValidation(int id)
        {
            return "value";
        }

        // POST api/<TicketController>
        [HttpPost]
        public async Task<FileContentResult> CreateTicketWithoutDesign([FromBody] Ticket ticket)
        {
            byte[] b = await _ticketService.CreateWithoutDesign(ticket.DelegateName,ticket.EventId);
            return  File(b, "application/octet-stream", "ticket.pdf");
        }

        // POST api/<TicketController>
        [HttpPost]
        public void CreateTicketWithDesign([FromBody] string value)
        {
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public void ConfirmTicket(int id, [FromBody] string value)
        {
        }
    }
}
