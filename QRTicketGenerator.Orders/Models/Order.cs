namespace QRTicketGenerator.Orders.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public Product Product { get; set; }

        public Order()
        {
            TotalPrice = Product.Price * Count;
        }
    }
}
