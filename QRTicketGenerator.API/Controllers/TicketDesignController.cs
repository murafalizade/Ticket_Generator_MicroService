using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.API.Dtos;
using QRTicketGenerator.API.Services;
using QRTicketGenerator.Shared.ControllerBases;
using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class TicketDesignController : CustomControllerBase
    {
        private readonly ITicketDesignService _ticketDesignService;
        public TicketDesignController(ITicketDesignService ticketDesignService)
        {
            _ticketDesignService = ticketDesignService;
        }

        // GET: api/<TicketDesignController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return CreateActionResultInstance(await _ticketDesignService.GetAll(userId));
        }

        // GET api/<TicketDesignController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return CreateActionResultInstance(await _ticketDesignService.GetById(id, userId));
        }

        // POST api/<TicketDesignController>
        [HttpPost]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public async Task<IActionResult> Post(IFormFile file, [FromQuery] TicketDesignDto value)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            // save file to uploads folder
            if(file == null)
            {
                return BadRequest("File is missing");
            }
            string fileName = Guid.NewGuid().ToString() + ".pdf";
            string filePath = "uploads/" + fileName;

            using (var ms = new MemoryStream())
            {
                file.CopyTo(ms);
                _ticketDesignService.PdfConverter(ms.ToArray(), filePath);
            }

            value.DesignFilePath = filePath;
            return CreateActionResultInstance(await _ticketDesignService.Create(value, userId));
        }

        // PUT api/<TicketDesignController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(IFormFile file, [FromQuery] UpdateTicketDesignDto value)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            // save file to uploads folder
            if (file != null)
            {
                string fileName = Guid.NewGuid().ToString() + ".pdf";
                string filePath = "uploads/" + fileName;
                using (var ms = new MemoryStream())
                {
                    file.CopyTo(ms);
                    _ticketDesignService.PdfConverter(ms.ToArray(), filePath);
                }
                value.DesignFilePath = filePath;
            }
            return CreateActionResultInstance(await _ticketDesignService.Update(value, userId));
        }

        // DELETE api/<TicketDesignController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return CreateActionResultInstance(await _ticketDesignService.Delete(id, userId));
        }
    }
}