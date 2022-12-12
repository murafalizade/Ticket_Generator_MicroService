using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using QRTicketGenerator.Orders.Models;

class ProductRepository:IProductRepository
{
    private readonly ApplicationDbContext _context;
    private readonly IMapper _mapper;
    public ProductRepository(ApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Product> GetProductById(int id)
    {
        return await _context.Products.FindAsync(id);
    }
    public async Task<List<Product>> GetAllProducts()
    {
        return await _context.Products.ToListAsync();
    }
    public async Task AddProduct(ProductsCreateDto obj)
    {
        Product product = _mapper.Map<Product>(obj);
        await _context.Products.AddAsync(product);
        await _context.SaveChangesAsync();
    }
    public async Task UpdateProduct(Product product)
    {
        _context.Products.Update(product);
        await _context.SaveChangesAsync();
    }
    public async Task DeleteProduct(int id)
    {
        var product = await _context.Products.FindAsync(id);
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
}