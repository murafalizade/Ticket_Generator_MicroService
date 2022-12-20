using System.Text.Json.Serialization;

namespace QRTicketGenerator.Orders.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Count { get; set; }
        public double TotalPrice { get; set; }
        [JsonIgnore]
        public int ProductId {get; set;}
        public Product Product { get; set; }
    }
}
