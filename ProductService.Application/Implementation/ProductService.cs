using ProductService.Application.DTO.Product;
using ProductService.Application.DTO.Product.Dto;
using ProductService.Application.Interface.Repositories;
using ProductService.Application.Interface.Services;
using ProductService.Application.Model.Result;
using ProductService.Domain.Model;

namespace ProductService.Application.Implementation;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryService _categoryService;
    private readonly ICompanyService _companyService;

    public ProductService(IProductRepository productRepository, ICategoryService categoryService, ICompanyService companyService)
    {
        _productRepository = productRepository;
        _categoryService = categoryService;
        _companyService = companyService;
    }
    public async Task<Result<Product>> CreateProduct(ProductDto productDto, string nameOfCategory, Guid workerId)
    {
        try
        {
            var company = await _companyService.GetCompanyByWorkerId(workerId);
            if (company is null)
            {
                Result<Product>.Failure($"You are not a worker of company, your id: {workerId}");
            }
            
            var category = await _categoryService.GetCategoryByName(nameOfCategory);
            if (category is null)
            {
                Result<Product>.Failure($"Can`t find category by name: {nameOfCategory}");
            }

            var modelOfProduct = ProductMapping.ToModel(productDto, category!, company!);

            var product = await _productRepository.CreateProduct(modelOfProduct);
            return Result<Product>.Success(product);
        }
        catch (Exception e)
        {
            return Result<Product>.Failure(e.ToString());
        }
    }
    
    public async Task<Result<Product>> ChangeProduct(Guid id, ProductDto productDto)
    {
        try
        {
            var product = await _productRepository.GetProductById(id);
            if (product is null)
            {
                Result<Product>.Failure($"Can`t find a product by id: {id}");
            }
            if (productDto.Name != product!.Name)
            {
                product.Name = productDto.Name;
            }

            if (productDto.Description != product.Description && product.Description != null)
            {
                product.Description = productDto.Description;
            }

            var result = await _productRepository.ChangeProduct(product);
            return Result<Product>.Success(result);
        }
        catch (Exception e)
        {
            return Result<Product>.Failure(e.ToString());
        }
    }

    public async Task<Result> DeleteProduct(Guid id)
    {
        try
        {
            var product = await _productRepository.GetProductById(id);
            if (product is null)
            {
                return Result.Failure($"Can`t find product by id: {id}");
            }
            await _productRepository.DeleteProduct(product);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.ToString());
        }
    }

    public async Task<Product?> GetProductById(Guid id)
    {
        var result = await _productRepository.GetProductById(id);
        return result;
    }
}