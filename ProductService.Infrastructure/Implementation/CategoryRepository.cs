using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interface.Repositories;
using ProductService.Domain.Model;

namespace ProductService.Infrastructure.Implementation;

public class CategoryRepository : ICategoryRepository
{
    private readonly AppDbContext _dbContext;

    public CategoryRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Category> CreateCategory(Category category)
    {
        await _dbContext.Categories.AddAsync(category);
        await _dbContext.SaveChangesAsync();
        return category;
    }

    public async Task<Category> ChangeCategory(Category category)
    {
        _dbContext.Categories.Update(category);
        await _dbContext.SaveChangesAsync();
        return category;    
    }

    public async Task<Category> DeleteCategory(Category category)
    {
        _dbContext.Categories.Remove(category);
        await _dbContext.SaveChangesAsync();
        return category;    
    }

    public async Task<Category?> GetCategoryById(Guid id)
    {
        var category = await _dbContext.Categories.FirstOrDefaultAsync(c => c.Id == id);
        return category;
    }

    public async Task<Category?> GetCategoryByName(string name)
    {
        var category = await _dbContext.Categories.FirstAsync(c => c.Name == name);
        return category;
        
    }
}