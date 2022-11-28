using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Services;
using System;
using System.IO;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        // POST api/<MailController>
        [HttpPost]
        public IActionResult SendMail(IFormFile file, [FromQuery] MailSenderDto obj)
        {
            try
            {
                var ms = new MemoryStream();
                file.CopyTo(ms);
                byte[] fileBytes = ms.ToArray();
                MailService.SendEmail(obj, fileBytes);
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
