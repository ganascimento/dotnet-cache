using Dotnet.Cache.Api.Domain;

namespace Dotnet.Cache.Api.Infra.Repository.interfaces;

public interface IEmployeeRepository
{
    Task<EmployeeEntity> GetByIdAsync(string id);
    Task<List<EmployeeEntity>> GetAllAsync();
    Task<EmployeeEntity> CreateAsync(EmployeeEntity entity);
}