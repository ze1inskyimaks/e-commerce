using System.ComponentModel.DataAnnotations;
using ProductService.Domain.Enum;

namespace ProductService.Domain.Model;

public class Product
{
    public Guid Id { get; set; } = Guid.NewGuid();
    [Required]
    public required string Name { get; set; }
    public Category? Category { get; set; }
    [Range(1,300)]
    public string? Description { get; set; }
    public List<string>? ImageUrl { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; } = ProductStatus.Open;
    [Required]
    public Guid CompanyId { get; set; }
    [Required]
    public required Company Company { get; set; }
}