using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotnet.Cache.Api.Domain;

public class EmployeeEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public required string Name { get; set; }
    public required DateTime BirthDate { get; set; }
    public required string Document { get; set; }
    public required string Phone { get; set; }
    public required string Company { get; set; }
}