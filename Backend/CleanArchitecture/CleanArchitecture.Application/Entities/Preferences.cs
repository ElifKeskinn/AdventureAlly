using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
// Turistlerin tercihlerini ve bildirim tercihlerini saklar. Turistlerin ilgi alanlarýný, dil tercihlerini ve favori mekanlarýný içerir.
namespace CleanArchitecture.Core.Entities
{
    public class Preferences : AuditableBaseEntity
    {
        public string UserId { get; set; }
        public User User { get; set; }
        public List<Interests> Interests { get; set; }
        public string PreferredLanguage { get; set; }
        public string Place { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }
    }
}
