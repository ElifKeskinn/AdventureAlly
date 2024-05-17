using System.Collections.Generic;
//Uygulamanın temel kullanıcılarını temsil eder. Turistlerin özelliklerini ve tercihlerini saklar.
namespace CleanArchitecture.Core.Entities
{
    public class User : AuditableBaseEntity
    {
        public UserPreferences UserPreferences { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public List<Interests> Interests { get; set; }
        public string PreferredLanguage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
   
}
