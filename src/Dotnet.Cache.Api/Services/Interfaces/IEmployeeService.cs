using Dotnet.Cache.Api.Domain;

namespace Dotnet.Cache.Api.Services.Interfaces;

public interface IEmployeeService
{
    Task<EmployeeEntity?> GetByIdAsync(string id);
    Task<List<EmployeeEntity>?> GetAllAsync();
    Task<EmployeeEntity> CreateAsync(EmployeeEntity entity);
}