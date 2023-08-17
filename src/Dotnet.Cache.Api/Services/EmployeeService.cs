using Dotnet.Cache.Api.Domain;
using Dotnet.Cache.Api.Infra.Repository.interfaces;
using Dotnet.Cache.Api.Services.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Dotnet.Cache.Api.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IMemoryCache _memoryCache;
    private readonly string ALL_KEY = "Employee_All_Key";

    public EmployeeService(IEmployeeRepository employeeRepository, IMemoryCache memoryCache)
    {
        _employeeRepository = employeeRepository;
        _memoryCache = memoryCache;
    }

    public async Task<EmployeeEntity?> GetByIdAsync(string id)
    {
        if (_memoryCache.TryGetValue<EmployeeEntity>(id, out EmployeeEntity? cachevalue))
            return cachevalue;

        await Task.Delay(1500);
        var employee = await _employeeRepository.GetByIdAsync(id);
        if (employee != null)
            _memoryCache.Set<EmployeeEntity>(id, employee, TimeSpan.FromSeconds(10));

        return employee;
    }

    public async Task<List<EmployeeEntity>?> GetAllAsync()
    {
        if (_memoryCache.TryGetValue<List<EmployeeEntity>>(ALL_KEY, out List<EmployeeEntity>? cacheValue))
            return cacheValue;

        await Task.Delay(1500);
        var employees = await _employeeRepository.GetAllAsync();
        if (employees != null && employees.Count > 0)
            _memoryCache.Set<List<EmployeeEntity>>(ALL_KEY, employees, TimeSpan.FromSeconds(10));

        return employees;
    }

    public Task<EmployeeEntity> CreateAsync(EmployeeEntity entity)
    {
        _memoryCache.Remove(ALL_KEY);
        return _employeeRepository.CreateAsync(entity);
    }
}