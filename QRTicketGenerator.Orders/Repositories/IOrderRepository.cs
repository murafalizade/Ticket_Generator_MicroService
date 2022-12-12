using System.Collections.Generic;
using System.Threading.Tasks;
using QRTicketGenerator.Orders.Models;

public interface IOrderRepository
{
    Task<Order> GetOrderById(int id);
    Task<List<Order>> GetAllOrders();
    Task AddOrder(OrderCreateDto order,string userId);
    Task UpdateOrder(Order order);
    Task DeleteOrder(int id);
}