using System.Text.Json.Serialization;

namespace QRTicketGenerator.Orders.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int MinCount { get; set; }
    }
}
