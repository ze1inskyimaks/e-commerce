using ProductService.Domain.Model;

namespace ProductService.Application.Interface.Repositories;

public interface ICompanyRepository
{
    public Task<Company> CreateCompany(Company company);
    public Task<Company> ChangeCompany(Company company);
    public Task<Company> DeleteCompany(Company company);
    public Task<Company?> GetCompanyByWorkerId(Guid workerId);
    public Task<Company?> GetCompanyById(Guid id);
}