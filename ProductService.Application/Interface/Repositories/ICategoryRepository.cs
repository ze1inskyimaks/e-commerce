using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Repositories;

public interface ICategoryRepository
{
    public Task<Category> CreateCategory(Category category);
    public Task<Category> ChangeCategory(Category category);
    public Task<Category> DeleteCategory(Category category);
    public Task<Category?> GetCategoryById(Guid id);
    public Task<Category?> GetCategoryByName(string name);
}