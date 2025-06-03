using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Repositories;

public interface IProductRepository
{
    public Task<Product> CreateProduct(Product product);
    public Task<Product> ChangeProduct(Product product);
    public Task<Product> DeleteProduct(Product id);
    public Task<Product?> GetProductById(Guid id);
}