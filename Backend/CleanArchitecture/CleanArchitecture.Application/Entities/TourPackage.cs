using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// Turistlere sunulan tur paketlerini temsil eder. Turistlere özel gezi veya etkinlikleri paketler halinde sunar.
namespace CleanArchitecture.Core.Entities
{
    public class TourPackage : AuditableBaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
}
