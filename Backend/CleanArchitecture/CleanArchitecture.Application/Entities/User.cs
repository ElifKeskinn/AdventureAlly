﻿using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
//Uygulamanın temel kullanıcılarını temsil eder. Turistlerin özelliklerini ve tercihlerini saklar.
namespace CleanArchitecture.Core.Entities
{
    public class User : AuditableBaseEntity
    {
       /* [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]**/

        // ObjectId olarak saklanacak ayrı bir alan
        public string PreferencesId { get; set; }
        public string Name { get; set; }
        public string Place { get; set; }
        public List<Interests> Interests { get; set; }
        public string PreferredLanguage { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
    }
   
}
