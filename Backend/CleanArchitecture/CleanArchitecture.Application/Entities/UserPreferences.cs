using System.Collections.Generic;
// Turistlerin tercihlerini ve bildirim tercihlerini saklar. Turistlerin ilgi alanlar�n�, dil tercihlerini ve favori mekanlar�n� i�erir.
namespace CleanArchitecture.Core.Entities
{
    public class UserPreferences
    {
        public List<string> Interests { get; set; }
        public string PreferredLanguage { get; set; }
        public string Place { get; set; }
        public List<string> FavoritePlaces { get; set; }
        public NotificationPreferences NotificationPreferences { get; set; }
    }
}
