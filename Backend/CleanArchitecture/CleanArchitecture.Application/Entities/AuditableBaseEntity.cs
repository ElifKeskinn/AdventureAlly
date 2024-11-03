using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CleanArchitecture.Core.Entities
{
    public abstract class AuditableBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public ObjectId Id { get; set; }
        public string CreatedBy { get; set; }
        public DateTime Created { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
    }
}
