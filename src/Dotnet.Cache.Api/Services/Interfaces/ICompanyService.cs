using Dotnet.Cache.Api.Domain;

namespace Dotnet.Cache.Api.Services.Interfaces;

public interface ICompanyService
{
    Task<CompanyEntity?> GetByIdAsync(string id);
    Task<List<CompanyEntity>?> GetAllAsync();
    Task<CompanyEntity> CreateAsync(CompanyEntity entity);
}