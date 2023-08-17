using Dotnet.Cache.Api.Domain;
using Dotnet.Cache.Api.Infra.Context;
using Dotnet.Cache.Api.Infra.Repository.interfaces;
using MongoDB.Driver;

namespace Dotnet.Cache.Api.Infra.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly MongoDbContext _context;

    public EmployeeRepository(MongoDbContext context)
    {
        _context = context;
    }

    public Task<EmployeeEntity> GetByIdAsync(string id)
    {
        return _context.Employee.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task<List<EmployeeEntity>> GetAllAsync()
    {
        return _context.Employee.Find(_ => true).ToListAsync();
    }

    public async Task<EmployeeEntity> CreateAsync(EmployeeEntity entity)
    {
        await _context.Employee.InsertOneAsync(entity);
        return entity;
    }
}