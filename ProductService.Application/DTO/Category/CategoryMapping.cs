using ProductService.Application.DTO.Category.Dto;

namespace ProductService.Application.DTO.Category;

public class CategoryMapping
{
    public static Domain.Model.Category ToModel(CategoryDto dto, Domain.Model.Category? category = null)
    {
        var result = new Domain.Model.Category()
        {
            Name = dto.Name,
            ParentCategoryId = category?.Id,
            ParentCategory = category
        };
        return result;
    }
}