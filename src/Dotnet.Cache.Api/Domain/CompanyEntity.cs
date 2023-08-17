using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotnet.Cache.Api.Domain;

public class CompanyEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required string FantasyName { get; set; }
    public required string Document { get; set; }
    public required string Address { get; set; }
    public required string Owner { get; set; }
}