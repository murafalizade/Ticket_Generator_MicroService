using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRTicketGenerator.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace QRTicketGenerator.Orders.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepository;

        public OrderController(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<List<Order>>> GetAllOrders()
        {
            return await _orderRepository.GetAllOrders();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrderById(int id)
        {
            return await _orderRepository.GetOrderById(id);
        }

        [HttpPost]
        public async Task<ActionResult<Order>> AddOrder(OrderCreateDto obj)
        {
            var userId = User.Claims.FirstOrDefault(x=>x.Type == ClaimTypes.NameIdentifier).Value;
            await _orderRepository.AddOrder(obj,userId);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Order>> UpdateOrder(Order order)
        {
            await _orderRepository.UpdateOrder(order);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Order>> DeleteOrder(int id)
        {
            await _orderRepository.DeleteOrder(id);
            return Ok();
        }
    }
}        