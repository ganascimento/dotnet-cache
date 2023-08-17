using Dotnet.Cache.Api.Domain;
using Dotnet.Cache.Api.Infra.Context;
using Dotnet.Cache.Api.Infra.Repositories.interfaces;
using MongoDB.Driver;

namespace Dotnet.Cache.Api.Infra.Repositories;

public class CompanyRepository : ICompanyRepository
{
    private readonly MongoDbContext _context;

    public CompanyRepository(MongoDbContext context)
    {
        _context = context;
    }

    public Task<CompanyEntity> GetByIdAsync(string id)
    {
        return _context.Company.Find(x => x.Id == id).FirstOrDefaultAsync();
    }

    public Task<List<CompanyEntity>> GetAllAsync()
    {
        return _context.Company.Find(_ => true).ToListAsync();
    }

    public async Task<CompanyEntity> CreateAsync(CompanyEntity entity)
    {
        await _context.Company.InsertOneAsync(entity);
        return entity;
    }
}