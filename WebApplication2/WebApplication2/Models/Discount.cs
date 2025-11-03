namespace WebApplication2.Models
{
    public class Discount
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public decimal Percent { get; set; } // Наприклад, 15 для 15% знижки
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
