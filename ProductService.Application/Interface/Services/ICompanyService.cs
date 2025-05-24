using ProductService.Application.DTO.Company.Dto;
using ProductService.Application.Model.Result;
using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Services;

public interface ICompanyService
{
    public Task<Result<Company>> CreateCompany(CompanyDto company, Guid ownerId);
    public Task<Result<Company>> ChangeCompany(Guid companyId, CompanyDto company, Guid ownerId);
    public Task<Result> DeleteCompany(Guid companyId, Guid ownerId);
    public Task<Company?> GetCompanyById(Guid companyId);
}