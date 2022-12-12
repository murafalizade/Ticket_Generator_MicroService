using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QRTicketGenerator.Orders.Models;

class OrderRepository :IOrderRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public OrderRepository(ApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task AddOrder(OrderCreateDto order,string userId)
    {
        var orderToAdd = _mapper.Map<Order>(order);
        orderToAdd.UserId = userId;
        await _context.Orders.AddAsync(orderToAdd);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteOrder(int id)
    {
        var orderToDelete = await _context.Orders.FindAsync(id);
        _context.Orders.Remove(orderToDelete);
        await _context.SaveChangesAsync();
    }
    public async Task<List<Order>> GetAllOrders()
    {
        return await _context.Orders.ToListAsync();
    }
    public async Task<Order> GetOrderById(int id)
    {
        return await _context.Orders.FindAsync(id);
    }
    public async Task UpdateOrder(Order order)
    {
        _context.Orders.Update(order);
        await _context.SaveChangesAsync();
    }
}