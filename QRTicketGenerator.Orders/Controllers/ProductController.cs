using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using QRTicketGenerator.Orders.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRTicketGenerator.Orders.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepository;

        public ProductController(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
       
        [HttpGet]
        public async Task<ActionResult<List<Product>>> GetAllProducts()
        {
            return await _productRepository.GetAllProducts();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            return await _productRepository.GetProductById(id);
        }

        [HttpPost]
        public async Task<ActionResult<Product>> AddProduct(ProductsCreateDto obj)
        {
            await _productRepository.AddProduct(obj);
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult<Product>> UpdateProduct(Product product)
        {
            await _productRepository.UpdateProduct(product);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Product>> DeleteProduct(int id)
        {
            await _productRepository.DeleteProduct(id);
            return Ok();
        }
    }
}
