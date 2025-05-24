using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Repositories;

public interface ICompanyRepository
{
    public Task<Company> CreateCompany(Company company);
    public Task<Company> ChangeCompany(Guid id, Company company);
    public Task<Company> DeleteCompany(Guid id);
    public Task<Company?> GetCompanyById(Guid id);
}