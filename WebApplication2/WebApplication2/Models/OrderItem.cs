namespace WebApplication2.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
<<<<<<< HEAD
        public string? Size { get; set; }
        public decimal Price { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
=======

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;
        public decimal Price { get; set; }
>>>>>>> 496c6cdd07bf6d142d4075783c173dccfadc866e
        public int Quantity { get; set; }
    }

}
