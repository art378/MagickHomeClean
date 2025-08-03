namespace WebApplication2.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        // Базова ціна товару без знижки
        public decimal OriginalPrice { get; set; }

        // Відсоток знижки (може бути null)
        public decimal? DiscountPercent { get; set; }

        // Розрахунок фактичної ціни з урахуванням знижки
        public decimal Price => HasDiscount
            ? Math.Round(OriginalPrice * (1 - DiscountPercent.Value / 100), 2)
            : OriginalPrice;

        // Кількість товару в кошику
        public int Quantity { get; set; }

        // Зображення товару
        public string? ImageUrl { get; set; }

        // Чи є активна знижка
        public bool HasDiscount => DiscountPercent.HasValue && DiscountPercent.Value > 0;
    }
}
