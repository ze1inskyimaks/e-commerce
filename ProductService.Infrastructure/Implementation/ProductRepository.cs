using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interface.Repositories;
using ProductService.Domain.Model;

namespace ProductService.Infrastructure.Implementation;

public class ProductRepository : IProductRepository
{
    private readonly AppDbContext _dbContext;

    public ProductRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Product> CreateProduct(Product product)
    {
        await _dbContext.Products.AddAsync(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> ChangeProduct(Product product)
    {
        _dbContext.Products.Update(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product> DeleteProduct(Product product)
    {
        _dbContext.Products.Remove(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        var product = await _dbContext.Products.FirstAsync(c => c.Id == id);
        return product;
    }
}