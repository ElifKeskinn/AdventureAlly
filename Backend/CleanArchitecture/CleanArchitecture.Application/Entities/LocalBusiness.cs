using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// Turistlere indirim sunan veya �zel anla�malar� olan yerel i�letmeleri temsil eder.
namespace CleanArchitecture.Core.Entities
{
    public class LocalBusiness : AuditableBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Name { get; set; }
        public string Place { get; set; }
        public string Category { get; set; }
        public List<Deal> Discounts { get; set; }
    }
}
