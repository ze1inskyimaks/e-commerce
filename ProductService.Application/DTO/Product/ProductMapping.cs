using ProductService.Application.DTO.Product.Dto;
using ProductService.Domain.Model;

namespace ProductService.Application.DTO.Product;

public static class ProductMapping
{
    public static Domain.Model.Product ToModel(ProductDto dto, Domain.Model.Category category, Domain.Model.Company company)
    {
        var result = new Domain.Model.Product()
        {
            Name = dto.Name,
            Category = category,
            CompanyId = company.Id,
            Company = company,
            Description = dto.Description,
            ImageUrl = dto.ImageUrl,
            Price = dto.Price,
            Quantity = dto.Quantity,
            Status = dto.Status
        };
        return result;
    }
}