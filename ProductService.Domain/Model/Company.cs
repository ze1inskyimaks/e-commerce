using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Model;

public class Company
{
    public Guid Id { get; private set; } = Guid.NewGuid();
    [Required]
    public required string Name { get; set; }
    public required Guid OwnerId { get; set; }
    [Range(1, 200)] 
    public List<Guid?> Workers { get; set; } = new();
    public string? Description { get; set; }
    public List<Guid>? ProductIds { get; set; }
    public List<Product>? Products { get; set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
}