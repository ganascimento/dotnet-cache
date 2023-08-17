using Dotnet.Cache.Api.Domain;

namespace Dotnet.Cache.Api.Infra.Repositories.interfaces;

public interface ICompanyRepository
{
    Task<CompanyEntity> GetByIdAsync(string id);
    Task<List<CompanyEntity>> GetAllAsync();
    Task<CompanyEntity> CreateAsync(CompanyEntity entity);
}