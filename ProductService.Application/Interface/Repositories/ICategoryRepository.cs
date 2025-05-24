using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Repositories;

public interface ICategoryRepository
{
    public Task<Category> CreateCategory(Category category);
    public Task<Category> ChangeCategory(Guid id, Category category);
    public Task<Category> DeleteCategory(Guid id);
    public Task<Category?> GetCategoryById(Guid id);
    public Task<Category?> GetCategoryByName(string name);
}