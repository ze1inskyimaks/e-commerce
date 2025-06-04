using Microsoft.EntityFrameworkCore;
using ProductService.Application.Interface.Repositories;
using ProductService.Domain.Model;

namespace ProductService.Infrastructure.Implementation;

public class CompanyRepository : ICompanyRepository
{
    private readonly AppDbContext _dbContext;

    public CompanyRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<Company> CreateCompany(Company company)
    {
        await _dbContext.Companies.AddAsync(company);
        await _dbContext.SaveChangesAsync();
        return company;
    }

    public async Task<Company> ChangeCompany(Company company)
    {
        _dbContext.Companies.Update(company);
        await _dbContext.SaveChangesAsync();
        return company;
    }

    public async Task<Company> DeleteCompany(Company company)
    {
        _dbContext.Companies.Remove(company);
        await _dbContext.SaveChangesAsync();
        return company;
    }

    public async Task<Company?> GetCompanyByWorkerId(Guid workerId)
    {
        var company = await _dbContext.Companies.FirstOrDefaultAsync(c => c.Workers.Contains(workerId));
        return company;
    }

    public async Task<Company?> GetCompanyById(Guid id)
    {
        var company = await _dbContext.Companies.FirstAsync(c => c.Id == id);
        return company;
    }
}