using System.ComponentModel.DataAnnotations;
using ProductService.Domain.Enum;
using ProductService.Domain.Model;

namespace ProductService.Application.DTO.Product.Dto;

public class ProductDto
{
    [Required]
    public required string Name { get; set; }
    [Range(1,300)]
    public string? Description { get; set; }
    public List<string>? ImageUrl { get; set; }
    public double Price { get; set; }
    public int Quantity { get; set; }
    public ProductStatus Status { get; set; }
}