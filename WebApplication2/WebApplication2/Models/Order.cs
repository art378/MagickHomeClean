using System.ComponentModel.DataAnnotations;

namespace WebApplication2.Models
{
    public enum OrderStatus
    {
        New,
        Processing,
        Completed
    }
    public class Order
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть ПІБ")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Будь ласка, введіть номер телефону")]
        [RegularExpression(@"^\+380\d{9}$", ErrorMessage = "Номер телефону повинен бути у форматі +380XXXXXXXXX")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Будь ласка, введіть адресу доставки")]
        public string Address { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public OrderStatus Status { get; set; } = OrderStatus.New;

        public List<OrderItem> Items { get; set; } = new();
    }

}
