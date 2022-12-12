using System.Collections.Generic;
using System.Threading.Tasks;
using QRTicketGenerator.Orders.Models;

public interface IProductRepository
{
    Task<Product> GetProductById(int id);
    Task<List<Product>> GetAllProducts();
    Task AddProduct(ProductsCreateDto product);
    Task UpdateProduct(Product product);
    Task DeleteProduct(int id);
}