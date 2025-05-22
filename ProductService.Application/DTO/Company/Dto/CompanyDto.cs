using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.DTO.Company.Dto;

public class CompanyDto
{
    [Required]
    public required string Name { get; set; }
    public required Guid OwnerId { get; set; }
    [Range(1,200)]
    public string? Description { get; set; }
}