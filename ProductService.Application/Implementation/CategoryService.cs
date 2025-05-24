using ProductService.Application.DTO.Category;
using ProductService.Application.DTO.Category.Dto;
using ProductService.Application.Interface.Repositories;
using ProductService.Application.Interface.Services;
using ProductService.Application.Model.Result;
using ProductService.Domain.Model;

namespace ProductService.Application.Implementation;

public class CategoryService : ICategoryService
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IProductService _productService;

    public CategoryService(ICategoryRepository categoryRepository, IProductService productService)
    {
        _categoryRepository = categoryRepository;
        _productService = productService;
    }
    
    public async Task<Result<Category>> CreateCategory(CategoryDto categoryDto, Guid? parentCategoryId = null)
    {
        Category categoryModel;
        Category? parentCategory;
        if (parentCategoryId == null)
        {
            categoryModel = CategoryMapping.ToModel(categoryDto);
        }
        else
        {
            parentCategory = await _categoryRepository.GetCategoryById(parentCategoryId.GetValueOrDefault());
            categoryModel = CategoryMapping.ToModel(categoryDto, parentCategory);
        }

        var categorySameByName = await _categoryRepository.GetCategoryByName(categoryModel.Name);
        
        if (categorySameByName != null)
        {
            return Result<Category>.Failure($"Cannot create a category, because a category with this name: {categoryDto.Name} already exist");
        }
        
        var result = await _categoryRepository.CreateCategory(categoryModel);

        if (parentCategoryId != null)
        {
            parentCategory = await _categoryRepository.GetCategoryById(parentCategoryId.GetValueOrDefault());
            parentCategory!.SubCategories.Add(result);
            await _categoryRepository.ChangeCategory(parentCategoryId.Value, parentCategory);
        }
        
        return Result<Category>.Success(result);
    }

    public async Task<Result<Category>> ChangeNameOfCategory(Guid id, CategoryDto categoryDto)
    {
        var categoryById = await _categoryRepository.GetCategoryById(id);

        if (categoryById == null)
        {
            return Result<Category>.Failure($"Cannot find category by id: {id}");
        }
        
        var categorySameByName = await _categoryRepository.GetCategoryByName(categoryDto.Name);
        
        if (categorySameByName != null)
        {
            return Result<Category>.Failure($"Cannot create a category, because a category with this name: {categoryDto.Name} already exist");
        }

        categoryById.Name = categoryDto.Name;

        var result = await _categoryRepository.ChangeCategory(id, categoryById);
        return Result<Category>.Success(result);
    }

    public async Task<Result> DeleteCategory(Guid id)
    {
        var categoryById = await _categoryRepository.GetCategoryById(id);

        if (categoryById == null)
        {
            return Result.Failure($"Cannot find category by id: {id}");
        }

        if (categoryById.SubCategories.Count != 0)
        {
            return Result.Failure("You need to first delete sub category form this category");
        }
        
        if (categoryById.ProductsIds!.Count != 0)
        {
            return Result.Failure($"You need to first delete category form products ids {categoryById.ProductsIds}");
        }

        if (categoryById.ParentCategoryId == null)
        {
            await _categoryRepository.DeleteCategory(id);
            return Result.Success();
        }
        else
        {
            var parentCategory = await _categoryRepository.GetCategoryById(categoryById.ParentCategoryId!.Value);
            if (parentCategory != null)
            {
                parentCategory.SubCategories.Remove(categoryById);
                await _categoryRepository.ChangeCategory(parentCategory.Id, parentCategory);
            }
            await _categoryRepository.DeleteCategory(id);
            return Result.Success();
        }
    }

    public async Task<Category?> GetCategoryById(Guid id)
    {
        var result = await _categoryRepository.GetCategoryById(id);
        return result;
    }

    public async Task<Result<Category>> AddProductInCategory(Guid categoryId, Guid productId)
    {
        var categoryById = await _categoryRepository.GetCategoryById(categoryId);

        if (categoryById == null)
        {
            return Result<Category>.Failure($"Cannot find category by id: {categoryId}");
        }

        var productById = await _productService.GetProductById(productId);
        
        if (productById == null)
        {
            return Result<Category>.Failure($"Cannot find product by id: {productId}");
        }

        categoryById.ProductsIds!.Add(productById.Id);
        categoryById.Products!.Add(productById);
        
        await _categoryRepository.ChangeCategory(categoryId, categoryById);
        return Result<Category>.Success(categoryById);
    }
    
    public async Task<Result<Category>> DeleteProductInCategory(Guid categoryId, Guid productId)
    {
        var categoryById = await _categoryRepository.GetCategoryById(categoryId);

        if (categoryById == null)
        {
            return Result<Category>.Failure($"Cannot find category by id: {categoryId}");
        }

        var productById = await _productService.GetProductById(productId);
        
        if (productById == null)
        {
            return Result<Category>.Failure($"Cannot find category by id: {productId}");
        }

        categoryById.ProductsIds!.Remove(productById.Id);
        categoryById.Products!.Remove(productById);
        
        await _categoryRepository.ChangeCategory(categoryId, categoryById);
        return Result<Category>.Success(categoryById);
    }
}
//TODO: Need to add a try-catch block on every function

/*public async Task<Result<Category>> AddOrChangeParentCategoryToCurrent(Guid currentCategoryId, Guid parentCategoryId)
{
    var currentCategory = await _categoryRepository.GetCategoryById(currentCategoryId);

    if (currentCategory == null)
    {
        Result<Category>.Failure($"Cannot find current category by id: {currentCategoryId}");
    }

    var parentCategory = await _categoryRepository.GetCategoryById(parentCategoryId);

    if (parentCategory == null)
    {
        Result<Category>.Failure($"Cannot find parent category by id: {parentCategoryId}");
    }

    currentCategory!.ParentCategoryIds = parentCategoryId;
    currentCategory.ParentCategory = parentCategory;

    await _categoryRepository.ChangeCategory(currentCategoryId, currentCategory);

    parentCategory!.SubCategories.(currentCategory);

    await _categoryRepository.ChangeCategory(parentCategoryId, parentCategory);
    return Result<Category>.Success(currentCategory);
}*/
