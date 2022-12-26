using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QRTicketGenerator.Orders.Models;
using QRTicketGenerator.Shared.Messages;
using System;
using System.Threading.Tasks;

namespace QRTicketGenerator.Orders.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class PaymentController : ControllerBase
    {
        private readonly  IOrderRepository _orderRepository;
        private readonly ISendEndpointProvider _sendEndPointProvider;

        public PaymentController(IOrderRepository orderRepository,ISendEndpointProvider sendEndpointProvider)
        {
            _orderRepository = orderRepository;
            _sendEndPointProvider = sendEndpointProvider;
        }
        [HttpPost("orderId")]
        public async Task<IActionResult> Pay(int orderId)
        {
            Order order = await _orderRepository.GetOrderById(orderId);
            var sendEndPoint = await _sendEndPointProvider.GetSendEndpoint(new Uri("queue:user-update-server"));
            UserCreateCommand userCreateCommand = new UserCreateCommand()
            {
                CoinCount = order.Count,
                Id = order.UserId,
                isPremium = true
            };
            await sendEndPoint.Send(userCreateCommand);
            return Ok();
        }
    }
}
