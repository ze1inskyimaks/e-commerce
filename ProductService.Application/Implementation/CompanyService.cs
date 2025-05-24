using ProductService.Application.DTO.Company;
using ProductService.Application.DTO.Company.Dto;
using ProductService.Application.Interface.Repositories;
using ProductService.Application.Interface.Services;
using ProductService.Application.Model.Result;
using ProductService.Domain.Model;

namespace ProductService.Application.Implementation;

public class CompanyService : ICompanyService
{
    private readonly ICompanyRepository _companyRepository;

    public CompanyService(ICompanyRepository companyRepository)
    {
        _companyRepository = companyRepository;
    }
    
    public async Task<Result<Company>> CreateCompany(CompanyDto company, Guid ownerId)
    {
        var companyModel = CompanyMapping.ToModel(company, ownerId);

        var result = await _companyRepository.CreateCompany(companyModel);
        
        return Result<Company>.Success(result);
    }

    public async Task<Result<Company>> ChangeCompany(Guid companyId, CompanyDto company, Guid ownerId)
    {
        var searchedCompany = await _companyRepository.GetCompanyById(companyId);

        if (searchedCompany == null)
        {
            return Result<Company>.Failure($"Can`t find a company by id: {companyId} for changing");
        }

        if (searchedCompany.OwnerId != ownerId)
        {
            return Result<Company>.Failure(
                $"You are a not a owner of a company. You written id: {ownerId}, and your written company id {companyId}");
        }
        
        var product = CompanyMapping.ToModel(company, ownerId);

        var result = await _companyRepository.CreateCompany(product);
        
        return Result<Company>.Success(result);
    }

    public async Task<Result> DeleteCompany(Guid companyId, Guid ownerId)
    {
        var searchedCompany = await _companyRepository.GetCompanyById(companyId);

        if (searchedCompany == null)
        {
            return Result.Failure($"Can`t find a company by id: {companyId} for changing");
        }

        if (searchedCompany.OwnerId != ownerId)
        {
            return Result.Failure(
                $"You are a not a owner of a company. You written id: {ownerId}, and your written company id {companyId}");
        }

        await _companyRepository.DeleteCompany(companyId);
        return Result.Success();
    }

    public async Task<Company?> GetCompanyById(Guid id)
    {
        var result = await _companyRepository.GetCompanyById(id);
        return result;
    }
}