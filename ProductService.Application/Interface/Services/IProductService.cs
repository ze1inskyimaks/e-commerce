using ProductService.Application.DTO.Product.Dto;
using ProductService.Application.Model.Result;
using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Services;

public interface IProductService
{
    public Task<Result<Product>> CreateProduct(ProductDto product, string nameOfCategory, Guid workerId);
    public Task<Result<Product>> ChangeProduct(Guid id, ProductDto product);
    public Task<Result> DeleteProduct(Guid id);
    public Task<Product?> GetProductById(Guid id);
}