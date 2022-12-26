using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.API.Services;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QRTicketGenerator.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    [ApiController]
    public class TicketImageController : ControllerBase
    {
        private readonly IImageService _imageService;
        public TicketImageController(IImageService imageService)
        {
            _imageService = imageService;
        }

        // GET api/<TicketImageController>/5
        [AllowAnonymous]
        [HttpGet("{filePath}")]
        public IActionResult Get(string filePath)
        {
            if(filePath == null)
            {
                return NotFound();
            }
            byte[] fileBytes =  _imageService.GetImageAsync(filePath, "uploads");
            //System.IO.File.ReadAllBytes("uploads/"+filePath);
            return  File(fileBytes, "image/png", "image.png");
        }

        // POST api/<TicketImageController>
        [AllowAnonymous]
        [HttpPost]
        [RequestSizeLimit(200 * 1024 * 1024)]
        [RequestFormLimits(MultipartBodyLengthLimit = 209715200)]
        public  async Task<IActionResult> Post(IFormFile file)
        {
            //var userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            // save file to uploads folder
            if (file == null)
            {
                return BadRequest("File is missing");
            }
            string filePath = await _imageService.UploadImage(file, "uploads");
            return Ok(filePath);
        }


        // DELETE api/<TicketImageController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
