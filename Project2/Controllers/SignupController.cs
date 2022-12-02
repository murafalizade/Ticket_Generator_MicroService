using MassTransit;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Project2.Models;
using QRTicketGenerator.IdentityServer.Dtos;
using QRTicketGenerator.Shared.Messages;
using System;
using System.Threading.Tasks;

namespace Project2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SignupController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ISendEndpointProvider _sendEndPointProvider;
        public SignupController(UserManager<ApplicationUser> userManager,ISendEndpointProvider sendEndpointProvider)
        {
            _userManager = userManager;
            _sendEndPointProvider = sendEndpointProvider;
        }

        [HttpGet("userId")]
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
        public async Task<IActionResult> Signup([FromBody] SignupDto singupDto)
        {
            var sendEndPoint = await _sendEndPointProvider.GetSendEndpoint(new Uri("queue:user-create-server"));
            ApplicationUser user = new ApplicationUser()
            {
                UserName = singupDto.UserName,
                Email = singupDto.Email,
            };
            UserCreateCommand userCreateCommand = new UserCreateCommand()
            {
                Id = user.Id,
                CoinCount = user.CoinCount,
                isPremium = user.IsPremium,
            };
            await sendEndPoint.Send(userCreateCommand);
            await _userManager.CreateAsync(user,singupDto.Password);
            
            return NoContent();
        }
    }
}
