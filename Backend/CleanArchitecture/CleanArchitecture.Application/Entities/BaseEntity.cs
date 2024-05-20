using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CleanArchitecture.Core.Entities
  
{
    public abstract class BaseEntity
    {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual int Id { get; set; }
    }
}
