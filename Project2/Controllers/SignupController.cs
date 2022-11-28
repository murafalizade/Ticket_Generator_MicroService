using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2.Models;
using QRTicketGenerator.IdentityServer.Dtos;
using System.Threading.Tasks;

namespace Project2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        public SignupController(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> GetUser(string userId)
        {
            ApplicationUser user = await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupDto singupDto)
        {
            ApplicationUser user = new ApplicationUser()
            {
                UserName = singupDto.UserName,
                Email = singupDto.Email,
            };
            await _userManager.CreateAsync(user,singupDto.Password);
            return NoContent();
        }
    }
}
