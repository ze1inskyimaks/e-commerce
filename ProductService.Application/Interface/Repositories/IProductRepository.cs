using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Repositories;

public interface IProductRepository
{
    public Task<Product> CreateProduct(Product product);
    public Task<Product> ChangeProduct(Guid id, Product product);
    public Task<Product> DeleteProduct(Guid id);
    public Task<Product?> GetProductById(Guid id);
}