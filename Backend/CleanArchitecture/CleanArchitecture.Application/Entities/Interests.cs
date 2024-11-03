using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace CleanArchitecture.Core.Entities
{
    public class Interests : BaseEntity

    {
       
        public string Nature { get; set; }
        public string Swimming { get; set; }
        public string Hiking { get; set; }
        public string Sking { get; set; }
        public string History { get; set; }

    }
}
