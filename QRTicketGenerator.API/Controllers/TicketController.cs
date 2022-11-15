using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.API.Models;
using QRTicketGenerator.API.Services;
using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task<FileContentResult> CreateTicketWithoutDesign([FromBody] Ticket ticket)
        {
            byte[] b = await _ticketService.CreateWithoutDesign(ticket.DelegateName,ticket.EventId);
            return  File(b, "application/octet-stream", "ticket.pdf");
        }

        // POST api/<TicketController>
        [HttpPost]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public async  Task<FileContentResult> CreateTicketWithDesign(IFormFile file,[FromQuery] Ticket ticket)
        {
            await using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream);
            byte[] c = memoryStream.ToArray();
            byte[] b = await _ticketService.CreateWithDesign(ticket.DelegateName, ticket.EventId, c);
            return File(b, "application/octet-stream", "ticket.pdf");
        }

        // PUT api/<TicketController>/5
        [HttpPut("{id}")]
        public IActionResult ConfirmTicket(string id)
        {
            _ticketService.ConfirmTicket(id);
            return NoContent();
        }
    }
}
