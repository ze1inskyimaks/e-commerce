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
        try
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
                return Result<Category>.Failure(
                    $"Cannot create a category, because a category with this name: {categoryDto.Name} already exist");
            }

            var result = await _categoryRepository.CreateCategory(categoryModel);

            if (parentCategoryId != null)
            {
                parentCategory = await _categoryRepository.GetCategoryById(parentCategoryId.GetValueOrDefault());
                parentCategory!.SubCategories.Add(result);
                await _categoryRepository.ChangeCategory(parentCategory);
            }

            return Result<Category>.Success(result);
        }
        catch (Exception e)
        {
            return Result<Category>.Failure(e.ToString());
        }
    }

    public async Task<Result<Category>> ChangeNameOfCategory(Guid id, CategoryDto categoryDto)
    {
        try
        {
            var categoryById = await _categoryRepository.GetCategoryById(id);

            if (categoryById == null)
            {
                return Result<Category>.Failure($"Cannot find category by id: {id}");
            }

            var categorySameByName = await _categoryRepository.GetCategoryByName(categoryDto.Name);

            if (categorySameByName != null)
            {
                return Result<Category>.Failure(
                    $"Cannot create a category, because a category with this name: {categoryDto.Name} already exist");
            }

            categoryById.Name = categoryDto.Name;

            var result = await _categoryRepository.ChangeCategory(categoryById);
            return Result<Category>.Success(result);
        }
        catch (Exception e)
        {
            return Result<Category>.Failure(e.ToString());
        }
    }

    public async Task<Result> DeleteCategory(Guid id)
    {
        try
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
                return Result.Failure(
                    $"You need to first delete category form products ids {categoryById.ProductsIds}");
            }

            if (categoryById.ParentCategoryId == null)
            {
                await _categoryRepository.DeleteCategory(categoryById);
                return Result.Success();
            }

            var parentCategory = await _categoryRepository.GetCategoryById(categoryById.ParentCategoryId!.Value);
            if (parentCategory != null)
            {
                parentCategory.SubCategories.Remove(categoryById);
                await _categoryRepository.ChangeCategory(parentCategory);
            }

            await _categoryRepository.DeleteCategory(categoryById);
            return Result.Success();
        }
        catch (Exception e)
        {
            return Result.Failure(e.ToString());
        }
    }

    public async Task<Category?> GetCategoryById(Guid id)
    {
        var result = await _categoryRepository.GetCategoryById(id);
        return result;
    }

    public async Task<Category?> GetCategoryByName(string nameOfCategory)
    {
        var result = await _categoryRepository.GetCategoryByName(nameOfCategory);
        return result;
    }

    public async Task<Result<Category>> AddProductInCategory(Guid categoryId, Guid productId)
    {
        try
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

            await _categoryRepository.ChangeCategory(categoryById);
            return Result<Category>.Success(categoryById);
        }
        catch (Exception e)
        {
            return Result<Category>.Failure(e.ToString());
        }
    }
    
    public async Task<Result<Category>> DeleteProductInCategory(Guid categoryId, Guid productId)
    {
        try
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

            await _categoryRepository.ChangeCategory(categoryById);
            return Result<Category>.Success(categoryById);
        }
        catch (Exception e)
        {
            return Result<Category>.Failure(e.ToString());
        }
    }
}

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
