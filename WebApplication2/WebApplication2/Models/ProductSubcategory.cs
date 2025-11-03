using WebApplication2.Models;

public class ProductSubcategory
{
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;

    public int SubcategoryId { get; set; }
    public Subcategory Subcategory { get; set; } = null!;
}