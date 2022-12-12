class OrderGetDto
{
        public int Id { get; set; }
        public int Count { get; set; }
        public decimal TotalPrice { get; set; }
        public ProductGetDto Product { get; set; }
}