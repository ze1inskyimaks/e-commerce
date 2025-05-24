using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Model;

public class Category
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string Name { get; set; }

    public List<Guid>? ProductsIds { get; set; } = new();
    public List<Product>? Products { get; set; } = new();
    public Guid? ParentCategoryId { get; set; }
    public Category? ParentCategory { get; set; }
    public List<Category> SubCategories { get; set; } = new();
}