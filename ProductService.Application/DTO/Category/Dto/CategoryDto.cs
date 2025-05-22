using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.DTO.Category.Dto;

public class CategoryDto
{
    [Required]
    public required string Name { get; set; }
    public Guid? ParentCategoryIds { get; set; }
    public CategoryDto? ParentCategory { get; set; }
}