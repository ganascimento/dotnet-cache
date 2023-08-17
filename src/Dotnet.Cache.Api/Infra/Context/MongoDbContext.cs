using Dotnet.Cache.Api.Domain;
using MongoDB.Driver;

namespace Dotnet.Cache.Api.Infra.Context;

public class MongoDbContext
{
    private readonly IMongoDatabase _database;

    public MongoDbContext(IConfiguration configuration)
    {
        var mongoClient = new MongoClient(configuration["Mongo:ConnectionString"]);
        _database = mongoClient.GetDatabase(configuration["Mongo:Database"]);
    }

    public IMongoCollection<EmployeeEntity> Employee
    {
        get
        {
            return _database.GetCollection<EmployeeEntity>("Employee");
        }
    }

    public IMongoCollection<CompanyEntity> Company
    {
        get
        {
            return _database.GetCollection<CompanyEntity>("Company");
        }
    }
}