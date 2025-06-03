using ProductService.Application.DTO.Category.Dto;
using ProductService.Application.Model.Result;
using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Services;

public interface ICategoryService
{
    public Task<Result<Category>> CreateCategory(CategoryDto category, Guid? categoryId = null);
    public Task<Result<Category>> ChangeNameOfCategory(Guid id, CategoryDto categoryDto);
    public Task<Result> DeleteCategory(Guid id);
    public Task<Category?> GetCategoryById(Guid id);
    public Task<Category?> GetCategoryByName(string nameOfCategory);

    public Task<Result<Category>> AddProductInCategory(Guid categoryId, Guid productId);
    public Task<Result<Category>> DeleteProductInCategory(Guid categoryId, Guid productId);

}